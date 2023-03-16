using System.Collections.Concurrent;

namespace FGLogDog.Application.Models
{
    internal static class Buffer
    {
        public static BlockingCollection<byte[]> buffer = new();
    }
}