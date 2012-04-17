using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetColorIndex : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetColorIndex.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                byte index = ReadUnsignedByte(stream);

                return new SetColorIndex(index);
            }
        }

        public static readonly byte Id = 0x63;

        public SetColorIndex(byte index)
            : this()
        {
            Index = index;
        }

        public readonly byte Index;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}