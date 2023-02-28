using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebAPI.EagleEye.Application.Contracts.Persistence;

namespace WebAPI.EagleEye.Application.Features.Queries.GetAllCollectionItems
{
    internal class GetCollectionQueryHandler : IRequestHandler<GetCollectionQuery, IReadOnlyList<object>>
    {
        private readonly ICollectionRepository _repository;

        public GetCollectionQueryHandler(ICollectionRepository repository)
            => _repository = repository;

        public async Task<IReadOnlyList<object>> Handle(GetCollectionQuery request, CancellationToken cancellationToken)
            => await _repository.GetAllAsync(request.CollectionName);
    }
}