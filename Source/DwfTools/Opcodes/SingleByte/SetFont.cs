using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetFont : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetFont.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                //TODO: Process *Not* entirely right I think.. check docs again

                ushort fields = ReadUnsignedShort(stream);
                string font = ReadString(stream);
                byte charset = ReadUnsignedByte(stream);
                byte pitch = ReadUnsignedByte(stream);
                byte family = ReadUnsignedByte(stream);
                byte style = ReadUnsignedByte(stream);
                uint height = ReadUnsignedLong(stream);
                ushort rotation = ReadUnsignedShort(stream);
                ushort widthScale = ReadUnsignedShort(stream);
                ushort spacing = ReadUnsignedShort(stream);
                ushort oblique = ReadUnsignedShort(stream);
                uint flags = ReadUnsignedLong(stream);

                byte[] bytes = new byte[4];
                stream.Read(bytes, 0, bytes.Length);

                return new SetFont(font,
                                   charset,
                                   pitch,
                                   family,
                                   style,
                                   height,
                                   rotation,
                                   widthScale,
                                   spacing,
                                   oblique,
                                   flags);
            }
        }

        public static readonly byte Id = 0x06;

        public SetFont(string font,
                       byte charset,
                       byte pitch,
                       byte family,
                       byte style,
                       uint height,
                       ushort rotation,
                       ushort widthScale,
                       ushort spacing,
                       ushort oblique,
                       uint flags)
            : this()
        {
            Font = font;
            Charset = charset;
            Pitch = pitch;
            Family = family;
            Style = style;
            Height = height;
            Rotation = rotation;
            WidthScale = widthScale;
            Spacing = spacing;
            Oblique = oblique;
            Flags = flags;
        }

        public readonly string Font;
        public readonly byte Charset;
        public readonly byte Pitch;
        public readonly byte Family;
        public readonly byte Style;
        public readonly uint Height;
        public readonly ushort Rotation;
        public readonly ushort WidthScale;
        public readonly ushort Spacing;
        public readonly ushort Oblique;
        public readonly uint Flags;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
