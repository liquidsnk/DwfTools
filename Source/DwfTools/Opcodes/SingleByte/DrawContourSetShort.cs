using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawContourSetShort : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawContourSetShort.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                int contourCount = ReadUnsignedByte(stream);

                //extended count
                if (contourCount == 0)
                {
                    contourCount = 256 + ReadUnsignedShort(stream);
                }

                for (int i = 0; i < contourCount; i++)
                {
                    //each contour has a list of points
                    int pointsCount = ReadUnsignedByte(stream);

                    //points extended count
                    if (pointsCount == 0)
                    {
                        pointsCount = 256 + ReadUnsignedShort(stream);
                    }

                    //first vertex
                    ReadSignedShort(stream);
                    ReadSignedShort(stream);

                    for (int j = 0; j < pointsCount; j++)
                    {
                        //vertex(j)
                        ReadSignedShort(stream);
                        ReadSignedShort(stream);
                    }
                }

                return new DrawContourSetShort();
            }
        }

        public static readonly byte Id = 0x0B;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
