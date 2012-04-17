using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawTexturedPolytriangle : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawTexturedPolytriangle.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                int pointCount = ReadUnsignedByte(stream);

                //extended count
                if (pointCount == 0)
                {
                    pointCount = 256 + ReadUnsignedShort(stream);
                }

                for (int i = 0; i < pointCount; i++)
                {
                    //point(i)
                    ReadSignedLong(stream);
                    ReadSignedLong(stream);
                }

                return new DrawTexturedPolytriangle();
            }
        }

        public static readonly byte Id = 0x77;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
