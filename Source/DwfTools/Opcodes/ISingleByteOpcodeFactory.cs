using System;
using System.IO;

namespace DwfTools.W2d.Opcodes
{
    public interface ISingleByteOpcodeFactory
    {
        int OpcodeId { get; }

        IOpcode ReadOpcode(Stream stream);
    }
}