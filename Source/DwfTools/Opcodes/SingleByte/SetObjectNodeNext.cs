using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetObjectNodeNext : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetObjectNodeNext.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                return new SetObjectNodeNext();
            }
        }

        public static readonly byte Id = 0x0E;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
