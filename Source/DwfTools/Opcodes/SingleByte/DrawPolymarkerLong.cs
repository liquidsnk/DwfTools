using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawPolymarkerLong : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawPolymarkerLong.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                int pointCount = ReadUnsignedByte(stream);

                //extended count
                if (pointCount  == 0)
                {
                    pointCount  = 256 + ReadUnsignedShort(stream);
                }

                var points = new Point[pointCount];
                for (int i = 0; i < pointCount ; i++)
                {
                    //point(i)
                    int x = ReadSignedLong(stream);
                    int y = ReadSignedLong(stream);
                    points[i] = new Point(x, y);
                }

                return new DrawPolymarkerLong(points);
            }
        }

        public static readonly byte Id = 0x6D;
        
        public DrawPolymarkerLong(IEnumerable<Point> points)
            : this()
        {
            Points = points.ToArray();
        }

        public readonly Point[] Points;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
