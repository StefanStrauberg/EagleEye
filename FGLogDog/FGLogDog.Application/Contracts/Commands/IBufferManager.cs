using MongoDB.Bson;

namespace FGLogDog.Application.Contracts.Commands
{
    internal interface IBufferManager
    {
        BsonDocument PullMessage();
    }
}
