using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawCircleLong : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawCircleLong.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                ReadSignedLong(stream);
                ReadSignedLong(stream);
                
                ReadUnsignedLong(stream);

                return new DrawCircleLong();
            }
        }

        public static readonly byte Id = 0x72;

        //TODO: Not supported for now

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
