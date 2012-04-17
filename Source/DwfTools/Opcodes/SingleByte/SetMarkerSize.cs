using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetMarkerSize : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetMarkerSize.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                uint markerSize = ReadUnsignedLong(stream);
                return new SetMarkerSize();
            }
        }

        public static readonly byte Id = 0x73;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
