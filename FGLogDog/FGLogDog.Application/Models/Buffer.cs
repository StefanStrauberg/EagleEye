using MongoDB.Bson;
using System.Collections.Concurrent;

namespace FGLogDog.Application.Models
{
    internal static class Buffer
    {
        public static BlockingCollection<BsonDocument> buffer = new();
    }
}