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
        private readonly ICollectionRepository  _repository;

        public CreateCollectionItemCommandHandler(ICollectionRepository repository)
            => _repository = repository;

        public async Task<Unit> Handle(CreateCollectionItemCommand request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(request.CollectionName,
                                          BsonSerializer.Deserialize<BsonDocument>(JsonSerializer.Serialize(request.JsonItem)));
            return Unit.Value;
        }
    }
}