using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetCurrentPoint : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetCurrentPoint.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                uint x = ReadUnsignedLong(stream);
                uint y = ReadUnsignedLong(stream);

                return new SetCurrentPoint(x, y);
            }
        }

        public static readonly byte Id = 0x4F;

        public SetCurrentPoint(uint x, uint y)
            : this()
        {
            X = x;
            Y = y;
        }

        public readonly uint X;

        public readonly uint Y;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Absolute; } }
    }
}
