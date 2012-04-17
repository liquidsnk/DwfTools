using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetFillModeOn : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetFillModeOn.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                return new SetFillModeOn();
            }
        }

        public static readonly byte Id = 0x46;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
