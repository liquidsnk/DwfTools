using System;
using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawTextAdvanced : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawTextAdvanced.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                //x, y
                int x = ReadSignedLong(stream);
                int y = ReadSignedLong(stream);

                string text = ReadString(stream);
                
                //** OVERSCORES **
                int overscoreCount = ReadUnsignedByte(stream);

                //extended count
                if (overscoreCount == 0)
                {
                    overscoreCount = 256 + ReadUnsignedShort(stream);
                }

                //the value is one-plus the number of overscores
                overscoreCount--;

                //get overscore position indexes
                for (int i = 0; i < overscoreCount; i++)
                {
                    int overscorePositionIndex = ReadUnsignedByte(stream);

                    //extended count
                    if (overscorePositionIndex == 0)
                    {
                        overscorePositionIndex = 256 + ReadUnsignedShort(stream);
                    }
                }

                //** UNDERSCORES **
                int underscoreCount = ReadUnsignedByte(stream);

                //extended count
                if (underscoreCount == 0)
                {
                    underscoreCount = 256 + ReadUnsignedShort(stream);
                }

                //the value is one-plus the number of underscores
                underscoreCount--;

                //get underscore position indexes
                for (int i = 0; i < underscoreCount; i++)
                {
                    int underscorePositionIndex = ReadUnsignedByte(stream);

                    //extended count
                    if (underscorePositionIndex == 0)
                    {
                        underscorePositionIndex = 256 + ReadUnsignedShort(stream);
                    }
                }

                //RES-count
                var reserved = ReadUnsignedByte(stream);

                //TODO: we assume it MUST be 1 and nothing comes next, proper support in the future
                if (reserved != 1) throw new InvalidOperationException();

                return new DrawTextAdvanced();
            }
        }

        public static readonly byte Id = 0x18;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
