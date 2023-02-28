using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using WebAPI.EagleEye.Application.Contracts.Persistence;

namespace EagleEye.Application.Features.Commands.UpdateCollectionItem
{
    internal class UpdateCollectionItemCommandHandler : IRequestHandler<UpdateCollectionItemCommand, Unit>
    {
        private readonly ICollectionRepository _repository;

        public UpdateCollectionItemCommandHandler(ICollectionRepository repository)
            => _repository = repository;

        public async Task<Unit> Handle(UpdateCollectionItemCommand request, CancellationToken cancellationToken)
        {
            await _repository.UpdateAsync(request.CollectionName,
                                          BsonSerializer.Deserialize<BsonDocument>(JsonSerializer.Serialize(request.JsonItem)));
            return Unit.Value;
        }
    }
}