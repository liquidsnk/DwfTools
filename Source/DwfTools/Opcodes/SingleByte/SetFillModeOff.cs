using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetFillModeOff : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetFillModeOff.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                return new SetFillModeOff();
            }
        }

        public static readonly byte Id = 0x66;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
