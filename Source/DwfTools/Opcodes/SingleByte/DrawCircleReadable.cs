using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawCircleReadable : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawCircleReadable.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                AdvanceThroughWhitespace(stream);

                ReadIntegerString(stream);
                ReadIntegerString(stream);

                AdvanceThroughWhitespace(stream);

                ReadIntegerString(stream);

                return new DrawCircleReadable();
            }
        }

        public static readonly byte Id = 0x52;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Absolute; } }
    }
}
