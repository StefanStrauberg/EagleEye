using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Contracts.Persistence;
using WebAPI.EagleEye.Application.Exceptions;

namespace EagleEye.Application.Features.Commands.UpdateCollectionItem
{
    internal class UpdateCollectionItemCommandHandler : IRequestHandler<UpdateCollectionItemCommand, Unit>
    {
        private readonly ICollectionRepository _repository;

        public UpdateCollectionItemCommandHandler(ICollectionRepository repository)
            => _repository = repository;

        public async Task<Unit> Handle(UpdateCollectionItemCommand request, CancellationToken cancellationToken)
        {
            var jsonData = JsonSerializer.Serialize(request.JsonItem);
            if (await _repository.UpdateAsync(request.CollectionName,
                                              request.Id,
                                              BsonSerializer.Deserialize<BsonDocument>(jsonData)))
                return Unit.Value;
            throw new NotFoundException(request.CollectionName, request.Id);
        }
    }
}