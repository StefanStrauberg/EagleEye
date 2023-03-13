using System.Collections.Concurrent;
using System.Text.Json.Nodes;

namespace FGLogDog.Application.Models
{
    public static class Buffer
    {
        public static BlockingCollection<JsonObject> buffer = new();
    }
}