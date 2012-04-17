using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawPolyBezierCurve : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawPolyBezierCurve.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                int curvesCount = ReadUnsignedByte(stream);

                //extended count
                if (curvesCount == 0)
                {
                    curvesCount = 256 + ReadUnsignedShort(stream);
                }

                //first point
                ReadSignedLong(stream);
                ReadSignedLong(stream);

                for (int i = 0; i < curvesCount; i++)
                {
                    //first control point
                    ReadSignedLong(stream);
                    ReadSignedLong(stream);

                    //second control point
                    ReadSignedLong(stream);
                    ReadSignedLong(stream);

                    //endpoint (beginning of next if any)
                    ReadSignedLong(stream);
                    ReadSignedLong(stream);
                }
                
                return new DrawPolyBezierCurve();
            }
        }

        public static readonly byte Id = 0x62;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
