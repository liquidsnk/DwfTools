using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawCirclePartial : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawCirclePartial.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                int x = ReadSignedLong(stream);
                int y = ReadSignedLong(stream);

                uint radius = ReadUnsignedLong(stream);

                ushort start = ReadUnsignedShort(stream);
                ushort end = ReadUnsignedShort(stream);

                return new DrawCirclePartial();
            }
        }

        public static readonly byte Id = 0x92;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
