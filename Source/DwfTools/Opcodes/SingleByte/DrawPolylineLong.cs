using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawPolylineLong : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawPolylineLong.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                int lineCount = ReadUnsignedByte(stream);

                //extended count
                if (lineCount == 0)
                {
                    lineCount = 256 + ReadUnsignedShort(stream);
                }

                var points = new Point[lineCount];
                for (int i = 0; i < lineCount; i++)
                {
                    //vertex(i)
                    int x = ReadSignedLong(stream);
                    int y = ReadSignedLong(stream);
                    points[i] = new Point(x, y);
                }

                return new DrawPolylineLong(points);
            }
        }

        public static readonly byte Id = 0x70;

        public DrawPolylineLong(IEnumerable<Point> points)
            : this()
        {
            Points = points.ToArray();
        }

        public readonly Point[] Points;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
