using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct SetLineWeight : IOpcode
    {
        public class Factory : OpcodeFactory, ISingleByteOpcodeFactory
        {
            public int OpcodeId { get { return SetLineWeight.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                int weight = ReadSignedLong(stream);

                return new SetLineWeight(weight);
            }
        }

        public static readonly byte Id = 0x17;
        
        public SetLineWeight(int weight)
            : this()
        {
            Weight = weight;
        }

        public readonly int Weight;

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
