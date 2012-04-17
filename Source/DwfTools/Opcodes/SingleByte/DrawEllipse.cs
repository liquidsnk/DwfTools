using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawEllipse : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawEllipse.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                int x = ReadSignedLong(stream);
                int y = ReadSignedLong(stream);

                uint rh = ReadUnsignedLong(stream);
                uint rv = ReadUnsignedLong(stream);

                ushort start = ReadUnsignedShort(stream);
                ushort end = ReadUnsignedShort(stream);
                ushort tilt = ReadUnsignedShort(stream);

                return new DrawEllipse();
            }
        }

        public static readonly byte Id = 0x65;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
