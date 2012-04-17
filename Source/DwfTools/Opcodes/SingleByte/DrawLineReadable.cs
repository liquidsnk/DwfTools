using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawLineReadable : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawLineReadable.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                AdvanceThroughWhitespace(stream);

                long x1 = ReadIntegerString(stream);
                long y1 = ReadIntegerString(stream);

                AdvanceThroughWhitespace(stream);

                long x2 = ReadIntegerString(stream);
                long y2 = ReadIntegerString(stream);

                return new DrawLineReadable(x1, y1, x2, y2);
            }
        }

        public static readonly byte Id = 0x4C;

        public DrawLineReadable(long x1, long y1, long x2, long y2)
            : this()
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public readonly long X1;
        public readonly long Y1;
        public readonly long X2;
        public readonly long Y2;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Absolute; } }
    }
}
