using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawPolylineShort : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawPolylineShort.Id; } }

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
                    short x = ReadSignedShort(stream);
                    short y = ReadSignedShort(stream);
                    points[i] = new Point(x, y);
                }

                return new DrawPolylineShort(points);
            }
        }

        public static readonly byte Id = 0x10;

        public DrawPolylineShort(IEnumerable<Point> points)
            : this()
        {
            Points = points.ToArray();
        }

        public readonly Point[] Points;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
