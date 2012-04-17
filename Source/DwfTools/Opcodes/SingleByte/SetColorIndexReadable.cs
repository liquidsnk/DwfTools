using System.Runtime.InteropServices;
using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetColorIndexReadable : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetColorIndexReadable.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                AdvanceThroughWhitespace(stream);

                var index = ReadIntegerString(stream);

                return new SetColorIndexReadable(index);
            }
        }

        public static readonly byte Id = 0x43;

        public SetColorIndexReadable(long index)
            : this()
        {
            Index = index;
        }

        public readonly long Index;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}