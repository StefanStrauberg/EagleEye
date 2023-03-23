using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Contracts.Persistence;

namespace WebAPI.EagleEye.Application.Features.Commands.CreateCollectionItem
{
    internal class CreateCollectionItemCommandHandler : IRequestHandler<CreateCollectionItemCommand, Unit>
    {
        readonly ICollectionRepository  _repository;

        public CreateCollectionItemCommandHandler(ICollectionRepository repository)
            => _repository = repository;

        public async Task<Unit> Handle(CreateCollectionItemCommand request, CancellationToken cancellationToken)
        {
            var jsonData = JsonSerializer.Serialize(request.JsonItem);
            await _repository.InsertOneAsync(request.CollectionName,
                                          BsonSerializer.Deserialize<BsonDocument>(jsonData));
            return Unit.Value;
        }
    }
}