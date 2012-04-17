using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Reflection;

namespace DwfTools.W2d.Opcodes
{
    public abstract class OpcodeFactory
    {
        [Serializable]
        public class DwfParsingException : Exception
        {
            public DwfParsingException() { }
            public DwfParsingException(string message) : base(message) { }
            public DwfParsingException(string message, Exception inner) : base(message, inner) { }
            protected DwfParsingException(
              SerializationInfo info,
              StreamingContext context)
                : base(info, context) { }
        }

        readonly static Dictionary<int, ISingleByteOpcodeFactory> _singleByteOpcodeFactories = new Dictionary<int, ISingleByteOpcodeFactory>();

        readonly static Dictionary<string, IExtendedAsciiOpcodeFactory> _extendedAsciiOpcodeFactories = new Dictionary<string, IExtendedAsciiOpcodeFactory>();

        static OpcodeFactory()
        {
            //Initialize single byte opcode factories
            var singleByteFactoryTypes = Assembly.GetAssembly(typeof(OpcodeFactory)).GetTypes()
                                                 .Where(t => t.IsClass)
                                                 .Where(t => !t.IsAbstract)
                                                 .Where(t => t.GetInterface(typeof(ISingleByteOpcodeFactory).Name) != null)
                                                 .Where(t => t.GetConstructor(Type.EmptyTypes) != null);

            foreach (var opcodeFactoryType in singleByteFactoryTypes)
            {
                var opcodeFactory = (ISingleByteOpcodeFactory)Activator.CreateInstance(opcodeFactoryType);

                if (_singleByteOpcodeFactories.ContainsKey(opcodeFactory.OpcodeId))
                    throw new InvalidOperationException(string.Format("A single byte opcode was already added to the list using the same id: {0}", opcodeFactory.OpcodeId));

                _singleByteOpcodeFactories[opcodeFactory.OpcodeId] = opcodeFactory;
            }

            //Initialize extended ascii opcode factories
            var extendedAsciiFactoryTypes = Assembly.GetAssembly(typeof(OpcodeFactory)).GetTypes()
                                                    .Where(t => t.IsClass)
                                                    .Where(t => !t.IsAbstract)
                                                    .Where(t => t.GetInterface(typeof(IExtendedAsciiOpcodeFactory).Name) != null)
                                                    .Where(t => t.GetConstructor(Type.EmptyTypes) != null);

            foreach (var opcodeFactoryType in extendedAsciiFactoryTypes)
            {
                var opcodeFactory = (IExtendedAsciiOpcodeFactory)Activator.CreateInstance(opcodeFactoryType);

                string opcodeId = opcodeFactory.OpcodeId;
                if (_extendedAsciiOpcodeFactories.ContainsKey(opcodeId))
                    throw new InvalidOperationException(string.Format("An extended ascii opcode was already added to the list using the same id: {0}", opcodeFactory.OpcodeId));

                _extendedAsciiOpcodeFactories[opcodeId] = opcodeFactory;
            }
        }
        
        public abstract IOpcode ReadOpcode(Stream stream);

        public const char extended_ascii_section_start_indicator = '(';

        public const char extended_ascii_section_end_indicator = ')';

        public const char extended_binary_section_start_indicator = '{';

        public const char extended_binary_section_end_indicator = '}';

        protected static bool ExtendedAsciiIsBeginning(Stream stream)
        {
            var readByte = stream.ReadByte();

            //Parse the opcode
            if (readByte == OpcodeFactory.extended_ascii_section_start_indicator)
            {
                return true;
            }

            stream.Position--;
            return false;
        }

        protected static bool ExtendedAsciiHasEnded(Stream stream)
        {
            AdvanceThroughWhitespace(stream);

            var readByte = (char)stream.ReadByte();
            bool hasEnded = (readByte == extended_ascii_section_end_indicator);
            if (hasEnded)
            {
                return true;
            }

            stream.Position--;
            return false;
        }

        public static IOpcode ParseSingleByteOpcode(int opcodeId, Stream stream)
        {
            ISingleByteOpcodeFactory opcodeFactory;
            if (!_singleByteOpcodeFactories.TryGetValue(opcodeId, out opcodeFactory))
            {
                throw new DwfParsingException(string.Format("Unrecognized single-byte opcode id: {0}", opcodeId));
            }

            var opcode = opcodeFactory.ReadOpcode(stream);
            return opcode;
        }

        public static IOpcode ReadExtendedAsciiOpcode(Stream stream)
        {
            string opcodeName = ReadExtendedAsciiOpcodeName(stream);
            
            //use the opcode name to find the correct factory
            if (_extendedAsciiOpcodeFactories.ContainsKey(opcodeName))
            {
                var factory = _extendedAsciiOpcodeFactories[opcodeName];

                return factory.ReadOpcode(stream);
            }
            else
            {
                var contents = PassThroughUnrecognizedExtendedAsciiOpcode(stream);

                Console.WriteLine(opcodeName + ": " + contents);
                return new UnrecognizedOpcode(opcodeName, contents);
            }
        }

        public static string PassThroughUnrecognizedExtendedAsciiOpcode(Stream stream)
        {
            //skip the rest of the opcode
            bool ignoreNextSpecialCharacter = false;
            int nestingLevel = 0;
            bool insideString = false;

            string wholeAsciiString = string.Empty;

            do
            {
                int extendedAsciiByte = stream.ReadByte();

                wholeAsciiString += ((char)extendedAsciiByte).ToString();

                //after a backslash, ignore special character properties
                if (ignoreNextSpecialCharacter)
                {
                    ignoreNextSpecialCharacter = false;
                    continue;
                }

                //if it's a quoted string, keep ignoring until it's closed
                if (insideString)
                {
                    if (extendedAsciiByte == '\'')
                    {
                        insideString = false;
                    }
                    continue;
                }

                if (extendedAsciiByte == extended_ascii_section_start_indicator)
                {
                    //open nested parens.
                    nestingLevel++;
                }
                else if (extendedAsciiByte == extended_ascii_section_end_indicator)
                {
                    //close nested parens, if this is the last close, we're finished
                    if (nestingLevel == 0)
                    {
                        break;
                    }

                    nestingLevel--;
                }
                else if (extendedAsciiByte == '\'')
                {
                    //determines beginning of a string.
                    insideString = true;
                }
                else if (extendedAsciiByte == '\\' && !ignoreNextSpecialCharacter)
                {
                    ignoreNextSpecialCharacter = true;
                }
                else if (extendedAsciiByte == extended_binary_section_start_indicator)
                {
                    //skip extended binary sections
                    ReadExtendedBinaryOpcode(stream);
                }
            }
            while (stream.Position < stream.Length);

            if (stream.Position > stream.Length)
            {
                throw new DwfParsingException("Gone past stream length while reading extended ascii opcode.");
            }

            return wholeAsciiString;
        }

        static string ReadExtendedAsciiOpcodeName(Stream stream)
        {
            AdvanceThroughWhitespace(stream);

            int extendedAsciiByte;
            string opcodeName = string.Empty;

            //get the opcode name
            do
            {
                extendedAsciiByte = stream.ReadByte();

                if (extendedAsciiByte == ' ' || extendedAsciiByte == extended_ascii_section_end_indicator) break;
                opcodeName += (char)extendedAsciiByte;
            }
            while (stream.Position < stream.Length);

            
            if (extendedAsciiByte == extended_ascii_section_end_indicator)
            {
                //if opcode is finished, leave the terminator there
                stream.Position--;
            }
            else if (stream.Position > stream.Length)
            {
                throw new DwfParsingException("Gone past stream length while reading extended ascii opcode name.");
            }
            else
            {
                AdvanceThroughWhitespace(stream);
            }

            return opcodeName;
        }

        public static IOpcode ReadExtendedBinaryOpcode(Stream stream)
        {
            //TODO: log: Console.WriteLine("Unrecognized extended binary operand detected, skipping.");

            //read the amount of bytes on the binary opcode
            int byteCount = ReadSignedLong(stream);

            if (byteCount == 0)
            {
                //byte count for the opcode equal zero?... all lies I say!, find out where the terminator is
                while (ReadChar(stream) != extended_binary_section_end_indicator);
            }
            else
            {
                //skip the binary section
                stream.Position += byteCount;
            }

            return new UnrecognizedOpcode(string.Empty);
        }

        public static char ReadChar(Stream stream)
        {
            return (char)stream.ReadByte();
        }

        public static byte ReadUnsignedByte(Stream stream)
        {
            int readByte = stream.ReadByte();
            return Convert.ToByte(readByte);
        }

        public static short ReadSignedShort(Stream stream)
        {
            byte[] readBytes = new byte[2];
            stream.Read(readBytes, 0, readBytes.Length);

            SwitchToBigEndianIfRequired(readBytes);

            return BitConverter.ToInt16(readBytes, 0);
        }

        public static ushort ReadUnsignedShort(Stream stream)
        {
            byte[] readBytes = new byte[2];
            stream.Read(readBytes, 0, readBytes.Length);

            SwitchToBigEndianIfRequired(readBytes);

            return BitConverter.ToUInt16(readBytes, 0);
        }

        public static int ReadSignedLong(Stream stream)
        {
            byte[] readBytes = new byte[4];
            stream.Read(readBytes, 0, readBytes.Length);

            SwitchToBigEndianIfRequired(readBytes);

            return BitConverter.ToInt32(readBytes, 0);
        }

        public static uint ReadUnsignedLong(Stream stream)
        {
            byte[] readBytes = new byte[4];
            stream.Read(readBytes, 0, readBytes.Length);

            SwitchToBigEndianIfRequired(readBytes);

            return BitConverter.ToUInt32(readBytes, 0);
        }

        public static long ReadIntegerString(Stream stream)
        {
            var validChars = new[] { '-', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            string readNumberString = string.Empty;
            do
            {
                var character = ReadChar(stream);
                if (validChars.Contains(character))
                {
                    readNumberString += character;
                }
                else
                {
                    break;
                }
            }
            while (stream.Position < stream.Length);

            return Convert.ToInt64(readNumberString);
        }

        public static string ReadString(Stream stream)
        {
            string resultingString = string.Empty;

            int firstByte = stream.ReadByte();
            if (firstByte == '\'')
            {
                bool ignoreNextSpecialCharacter = false;

                char stringChar;
                while (((stringChar = ReadChar(stream)) != '\'') || ignoreNextSpecialCharacter)
                {
                    ignoreNextSpecialCharacter = false;

                    if (stringChar == '\\' && !ignoreNextSpecialCharacter)
                    {
                        ignoreNextSpecialCharacter = true;
                        continue;
                    }

                    resultingString += stringChar;
                }

                return resultingString;
            }
            else if (firstByte == '\"')
            {
                //TODO: Implement
            }
            else
            {
                char stringChar = (char)firstByte;
                do
                {
                    resultingString += stringChar;
                    stringChar = ReadChar(stream);
                }
                while (!IsWhitespace(stringChar) && stringChar != extended_ascii_section_end_indicator);

                if (stringChar == extended_ascii_section_end_indicator)
                {
                    stream.Position--;
                }

                return resultingString;
            }

            //TODO: Actually read a string

            //ASCII or UNICODE text string. 
            //If the string is ASCII and does not contain any white spaces, 
            //the string is not enclosed in quotes. 
            //If the string is ASCII and contains at least one white space character,
            //the string is enclosed in single quotes ('). 
            //If the string is UNICODE, the string is hexadecimal encoded and enclosed
            //in double quotes (").

            return null;
        }

        public static void AdvanceThroughWhitespace(Stream stream)
        {
            while (IsWhitespace(ReadChar(stream)));

            stream.Position--;
        }

        public static bool IsWhitespace(char character)
        {
            var whitespaceChars = new[] { ' ', '\n', '\r', '\t' };

            return whitespaceChars.Contains(character);
        }

        private static void SwitchToBigEndianIfRequired(byte[] bytes)
        {
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
        }
    }
}
