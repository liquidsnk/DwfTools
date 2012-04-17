using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetMarkerGlyph : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetMarkerGlyph.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                uint markerGlyph = ReadUnsignedLong(stream);

                return new SetMarkerGlyph();
            }
        }

        public static readonly byte Id = 0x87;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
