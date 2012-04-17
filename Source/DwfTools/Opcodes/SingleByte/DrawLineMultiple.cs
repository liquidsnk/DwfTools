using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawLineMultiple : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            static readonly DrawLineShort.Factory _DrawLineShortFactory = new DrawLineShort.Factory();

            public int OpcodeId { get { return DrawLineMultiple.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                byte lineCount = ReadUnsignedByte(stream);
                
                var lines = new DrawLineShort[lineCount];
                
                for (int i = 0; i < lineCount; i++)
                {
                    lines[i] = (DrawLineShort)_DrawLineShortFactory.ReadOpcode(stream);
                }

                return new DrawLineMultiple(lines);
            }
        }

        public static readonly byte Id = 0x8C;

        public DrawLineMultiple(IEnumerable<DrawLineShort> lines)
            : this()
        {
            Lines = lines.ToArray();
        }

        public readonly DrawLineShort[] Lines;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
