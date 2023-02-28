using MediatR;
using MongoDB.Bson;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Contracts.Persistence;

namespace WebAPI.EagleEye.Application.Features.Queries.GetAllCollectionItems
{
    internal class GetCollectionQueryHandler : IRequestHandler<GetCollectionQuery, string>
    {
        private readonly ICollectionRepository _repository;

        public GetCollectionQueryHandler(ICollectionRepository repository)
            => _repository = repository;

        public async Task<string> Handle(GetCollectionQuery request, CancellationToken cancellationToken)
            => (await _repository.GetAllAsync(request.CollectionName)).ToJson();
    }
}