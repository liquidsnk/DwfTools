using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawGouraudPolytriangleShort : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawGouraudPolytriangleShort.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                int triangleCount = ReadUnsignedByte(stream);

                //extended count
                if (triangleCount == 0)
                {
                    triangleCount = 256 + ReadUnsignedShort(stream);
                }

                //vertex1
                ReadSignedShort(stream);
                ReadSignedShort(stream);

                //vertex1 color
                ReadUnsignedLong(stream);

                //vertex2
                ReadSignedShort(stream);
                ReadSignedShort(stream);

                //vertex2 color
                ReadUnsignedLong(stream);

                for (int i = 0; i < triangleCount; i++)
                {
                    //vertex(i)
                    ReadSignedShort(stream);
                    ReadSignedShort(stream);

                    //vertex(i) color
                    ReadUnsignedLong(stream);
                }

                return new DrawGouraudPolytriangleShort();
            }
        }

        public static readonly byte Id = 0x07;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
