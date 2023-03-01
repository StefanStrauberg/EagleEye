using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Contracts.Persistence;
using WebAPI.EagleEye.Application.Exceptions;

namespace WebAPI.EagleEye.Application.Features.Queries.GetCollectionItem
{
    internal class GetCollectionItemByIdQueryHandler : IRequestHandler<GetCollectionItemByIdQuery, string>
    {
        private readonly ICollectionRepository _repository;

        public GetCollectionItemByIdQueryHandler(ICollectionRepository repository)
            => _repository = repository;

        public async Task<string> Handle(GetCollectionItemByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _repository.GetByIdAsync(request.CollectionName, request.Id);
            if (data is null)
                throw new NotFoundException(request.CollectionName, request.Id);
            var jsonWritersetting = new JsonWriterSettings() { OutputMode = JsonOutputMode.Strict };
            return data.ToJson(jsonWritersetting);
        }
    }
}