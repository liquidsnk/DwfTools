using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetColorRgba : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetColorRgba.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                byte[] bytes = new byte[4];
                stream.Read(bytes, 0, bytes.Length);

                return new SetColorRgba(bytes[0], bytes[1], bytes[2], bytes[3]);
            }
        }

        public static readonly byte Id = 0x03;
        
        public SetColorRgba(byte r, byte g, byte b, byte a)
            : this()
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public readonly byte R;
        public readonly byte G;
        public readonly byte B;
        public readonly byte A;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
