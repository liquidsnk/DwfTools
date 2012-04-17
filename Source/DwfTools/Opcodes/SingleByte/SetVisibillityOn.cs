using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetVisibillityOn : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetVisibillityOn.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                return new SetVisibillityOn();
            }
        }

        public static readonly byte Id = 0x56;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
