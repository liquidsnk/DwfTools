using System;
using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public interface IExtendedAsciiOpcodeFactory
    {
        string OpcodeId { get; }

        IOpcode ReadOpcode(Stream stream);
    }
}
