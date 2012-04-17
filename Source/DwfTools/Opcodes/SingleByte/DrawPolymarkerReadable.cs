using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawPolymarkerReadable : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawPolymarkerReadable.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {

                AdvanceThroughWhitespace(stream);

                long markerCount = ReadIntegerString(stream);

                AdvanceThroughWhitespace(stream);

                for (int i = 0; i < markerCount; i++)
                {
                    //vertex(i)
                    ReadIntegerString(stream);
                    ReadIntegerString(stream);
                }

                return new DrawPolymarkerReadable();
            }
        }

        public static readonly byte Id = 0x4D;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Absolute; } }
    }
}
