using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawPolylinePolygonReadable : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawPolylinePolygonReadable.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                AdvanceThroughWhitespace(stream);

                long lineCount = ReadIntegerString(stream);
                
                for (int i = 0; i < lineCount; i++)
                {
                    AdvanceThroughWhitespace(stream);

                    //vertex(i)
                    ReadIntegerString(stream);
                    ReadIntegerString(stream);
                }

                return new DrawPolylinePolygonReadable();
            }
        }

        public static readonly byte Id = 0x50;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Absolute; } }
    }
}
