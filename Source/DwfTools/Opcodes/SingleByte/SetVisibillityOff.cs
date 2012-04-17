using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetVisibillityOff : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetVisibillityOff.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                return new SetVisibillityOff();
            }
        }

        public static readonly byte Id = 0x76;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
