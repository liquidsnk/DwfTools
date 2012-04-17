using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawLineLong : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawLineLong.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                int x1 = ReadSignedLong(stream);
                int y1 = ReadSignedLong(stream);
                int x2 = ReadSignedLong(stream);
                int y2 = ReadSignedLong(stream);

                return new DrawLineLong(x1, y1, x2, y2);
            }
        }

        public static readonly byte Id = 0x6C;

        public DrawLineLong(int x1, int y1, int x2, int y2)
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
