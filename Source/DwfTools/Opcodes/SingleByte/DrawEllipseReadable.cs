using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawEllipseReadable : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawEllipseReadable.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                AdvanceThroughWhitespace(stream);

                long x = ReadIntegerString(stream);
                long y = ReadIntegerString(stream);

                AdvanceThroughWhitespace(stream);

                long rh = ReadIntegerString(stream);
                long rv = ReadIntegerString(stream);

                return new DrawEllipseReadable();
            }
        }

        public static readonly byte Id = 0x45;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Absolute; } }
    }
}
