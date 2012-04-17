using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetMarkerGlyphReadable : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetMarkerGlyphReadable.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                AdvanceThroughWhitespace(stream);

                long markerGlyph = ReadIntegerString(stream);

                return new SetMarkerGlyphReadable();
            }
        }

        public static readonly byte Id = 0x47;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
