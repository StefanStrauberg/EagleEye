using System;

namespace EagleEye.TemporaryBuffer.Config
{
    internal class BufferConfiguration : IBufferConfiguration
    {
        public int SizeOfBuffer { get; set; }
        public string CollectionNameIndex { get; set; }
    }
}