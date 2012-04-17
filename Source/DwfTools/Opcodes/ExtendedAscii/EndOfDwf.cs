using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public struct EndOfDwf : IOpcode
    {
        public class Factory : OpcodeFactory, IExtendedAsciiOpcodeFactory
        {
            public string OpcodeId { get { return EndOfDwf.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                return new EndOfDwf();
            }
        }

        public static readonly string Id = "EndOfDWF";

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
