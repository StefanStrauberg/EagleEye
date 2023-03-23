using MongoDB.Bson;

namespace WebAPI.EagleEye.Application.Contracts.Persistence
{
    public interface ICollectionRepository : IGenericRepository<BsonDocument>
    {
    }
}