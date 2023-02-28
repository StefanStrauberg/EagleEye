using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebAPI.EagleEye.Application.Contracts.Persistence;

namespace WebAPI.EagleEye.Application.Features.Queries.GetCollectionItem
{
    internal class GetCollectionItemByIdQueryHandler : IRequestHandler<GetCollectionItemByIdQuery, Object>
    {
        private readonly ICollectionRepository _repository;

        public GetCollectionItemByIdQueryHandler(ICollectionRepository repository)
            => _repository = repository;

        public async Task<object> Handle(GetCollectionItemByIdQuery request, CancellationToken cancellationToken)
            => await _repository.GetByIdAsync(request.CollectionName, request.Id);
    }
}