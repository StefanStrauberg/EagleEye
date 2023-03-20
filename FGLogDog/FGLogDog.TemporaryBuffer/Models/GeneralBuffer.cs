using System.Collections.Concurrent;

namespace FGLogDog.TemporaryBuffer.Models
{
    internal static class GeneralBuffer
    {
        internal static BlockingCollection<byte[]> buffer = new BlockingCollection<byte[]>();
    }
}