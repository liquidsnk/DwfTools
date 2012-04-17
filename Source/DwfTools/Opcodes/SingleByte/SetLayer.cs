using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetLayer : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetLayer.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                int layerNumber = ReadUnsignedByte(stream);

                //extended count
                if (layerNumber == 0)
                {
                    layerNumber = 256 + ReadUnsignedShort(stream);
                }

                return new SetLayer();
            }
        }

        public static readonly byte Id = 0xAC;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
