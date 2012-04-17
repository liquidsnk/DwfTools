using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawPolymarkerShort : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawPolymarkerShort.Id; } }

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
                    short x = ReadSignedShort(stream);
                    short y = ReadSignedShort(stream);
                    points[i] = new Point(x, y);
                }

                return new DrawPolymarkerShort(points);
            }
        }

        public static readonly byte Id = 0x8D;
        
        public DrawPolymarkerShort(IEnumerable<Point> points)
            : this()
        {
            Points = points.ToArray();
        }

        public readonly Point[] Points;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
