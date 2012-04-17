using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetMarkerSizeReadable : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetMarkerSizeReadable.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                AdvanceThroughWhitespace(stream);

                long markerSize = ReadIntegerString(stream);
                return new SetMarkerSizeReadable();
            }
        }

        public static readonly byte Id = 0x53;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
