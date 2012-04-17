using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawGouraudPolytriangleLong : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawGouraudPolytriangleLong.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                int triangleCount = ReadUnsignedByte(stream);
                
                //extended count
                if (triangleCount == 0)
                {
                    triangleCount = 256 + ReadUnsignedShort(stream);
                }

                //vertex1
                ReadSignedLong(stream);
                ReadSignedLong(stream);

                //vertex1 color
                ReadUnsignedLong(stream);

                //vertex2
                ReadSignedLong(stream);
                ReadSignedLong(stream);

                //vertex2 color
                ReadUnsignedLong(stream);

                for (int i = 0; i < triangleCount; i++)
                {
                    //vertex(i)
                    ReadSignedLong(stream);
                    ReadSignedLong(stream);

                    //vertex(i) color
                    ReadUnsignedLong(stream);
                }

                return new DrawGouraudPolytriangleLong();
            }
        }

        public static readonly byte Id = 0x67;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
