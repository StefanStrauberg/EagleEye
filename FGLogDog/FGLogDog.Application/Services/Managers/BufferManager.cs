using FGLogDog.Application.Contracts.Commands;
using FGLogDog.Application.Models;
using MongoDB.Bson;

namespace FGLogDog.Application.Services.Managers
{
    internal class BufferManager : IBufferManager
    {
        public BsonDocument PullMessage()
        {
            while (!Buffer.buffer.IsCompleted)
            {
                if (Buffer.buffer.TryTake(out BsonDocument result))
                    return result;
            }
            return null;
        }
    }
}
