using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetObjectNodeLong : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetObjectNodeLong.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                uint nodeNumber = ReadUnsignedLong(stream);

                return new SetObjectNodeLong(nodeNumber);
            }
        }

        public static readonly byte Id = 0x4E;

        public SetObjectNodeLong(uint nodeNumber)
            : this()
        {
            NodeNumber = nodeNumber;
        }

        public readonly uint NodeNumber;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
