namespace DwfTools.W2d.Opcodes
{
    public class UnrecognizedOpcode : IOpcode
    {
        public UnrecognizedOpcode(string id)
        {
            OpcodeId = id;
        }

        public UnrecognizedOpcode(string id, string content)
            : this(id)
        {
            Content = content;
        }

        public string OpcodeId { get; set; }

        public string Content { get; set; }

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
