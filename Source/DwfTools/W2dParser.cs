using System.Collections.Generic;
using System.IO;
using System.Linq;
using DwfTools.W2d.Opcodes;
using System;

namespace DwfTools
{
    public class W2dParser
    {
        public class W2dParserResults : List<IOpcode>
        {
            public W2dParserResults(IEnumerable<IOpcode> collection)
                : base(collection)
            {
            }

            public W2dParserResults()
            {
            }

            public W2dParserResults(int capacity)
                : base(capacity)
            {
            }
        }

        public static W2dParserResults GetParsedData(string fileName)
        {
            var parsedOpcodes = new W2dParserResults(GetParsedIterator(fileName).ToList());

            return parsedOpcodes;
        }

        public const int w2d_header_length = 12;

        public static IEnumerable<IOpcode> GetParsedIterator(string fileName)
        {
            var file = File.OpenRead(fileName);
            file.Position = w2d_header_length; //skip header

            return GetParsedIteratorIterable(file);
        }
        
        static IEnumerable<IOpcode> GetParsedIteratorIterable(Stream stream)
        {
            while (stream.Position < stream.Length )
            {
                var opcode = ParseNextOpcode(stream);
                if (opcode is EndOfDwf) yield break;

                yield return opcode;
            }
        }

        public static IOpcode ParseNextOpcode(Stream stream)
        {
            //Get next byte to identify following (opcode / opcode type) in the stream
            var readByte = stream.ReadByte();

            //Parse the opcode
            if (readByte == OpcodeFactory.extended_ascii_section_start_indicator)
            {
                return OpcodeFactory.ReadExtendedAsciiOpcode(stream);
            }
            else if (readByte == OpcodeFactory.extended_binary_section_start_indicator)
            {
                return OpcodeFactory.ReadExtendedBinaryOpcode(stream);
            }
            else
            {
                return OpcodeFactory.ParseSingleByteOpcode(readByte, stream);
            }
        }
    }
}
