using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawCircleShort : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawCircleShort.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                ReadSignedShort(stream);
                ReadSignedShort(stream);
                
                ReadUnsignedShort(stream);

                return new DrawCircleShort();
            }
        }

        public static readonly byte Id = 0x12;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
