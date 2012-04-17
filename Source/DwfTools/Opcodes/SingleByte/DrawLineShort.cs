using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawLineShort : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawLineShort.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                short x1 = ReadSignedShort(stream);
                short y1 = ReadSignedShort(stream);
                short x2 = ReadSignedShort(stream);
                short y2 = ReadSignedShort(stream);

                return new DrawLineShort(x1, y1, x2, y2);
            }
        }

        public static readonly byte Id = 0x0C;

        public DrawLineShort(short x1, short y1, short x2, short y2)
            : this()
        {
            StartingPoint = new Point(x1, y1);
            EndPoint = new Point(x2, y2);
        }

        public readonly Point StartingPoint;
        public readonly Point EndPoint;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
