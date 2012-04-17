using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetObjectNodeShort : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetObjectNodeShort.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                short nodeNumber = ReadSignedShort(stream);

                return new SetObjectNodeShort(nodeNumber);
            }
        }

        public static readonly byte Id = 0x6E;

        public SetObjectNodeShort(short nodeNumber)
            : this()
        {
            NodeNumber = nodeNumber;
        }

        public readonly short NodeNumber;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Relative; } }
    }
}
