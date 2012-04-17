using System;
using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct DrawTextBasic : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return DrawTextBasic.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                //x, y
                int x = ReadSignedLong(stream);
                int y = ReadSignedLong(stream);

                string text = ReadString(stream);
                
                return new DrawTextBasic(new Point(x, y), text);
            }
        }

        public static readonly byte Id = 0x78;
        
        public DrawTextBasic(Point position, string text)
            : this()
        {
            Position = position;
            Text = text;
        }

        public readonly Point Position;
        public readonly string Text;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
