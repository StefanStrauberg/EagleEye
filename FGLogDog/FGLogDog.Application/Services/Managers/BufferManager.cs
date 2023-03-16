using FGLogDog.Application.Contracts.Commands;
using MongoDB.Bson;
using System.Threading;
using Buffer = FGLogDog.Application.Models.Buffer;

namespace FGLogDog.Application.Services.Managers
{
    internal class BufferManager : IBufferManager
    {
        public BsonDocument TakeMessage()
        {
            while (!Buffer.buffer.IsCompleted)
            {
                if (Buffer.buffer.Count > 0)
                    if(Buffer.buffer.TryTake(out BsonDocument document))
                        return document;
                Thread.Sleep(1000);
            }
            return null;
        }
    }
}
