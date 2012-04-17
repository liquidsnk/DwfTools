using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawPolytriangleReadable : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawPolytriangleReadable.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                AdvanceThroughWhitespace(stream);

                long pointCount = ReadIntegerString(stream);

                for (int i = 0; i < pointCount; i++)
                {
                    AdvanceThroughWhitespace(stream);

                    //point(i) coordinates
                    long x = ReadIntegerString(stream);
                    long y = ReadIntegerString(stream);
                }

                return new DrawPolytriangleReadable();
            }
        }

        public static readonly byte Id = 0x54;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Absolute; } }
    }
}
