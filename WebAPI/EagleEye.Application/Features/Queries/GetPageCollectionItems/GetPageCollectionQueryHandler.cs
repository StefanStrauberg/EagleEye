using EagleEye.Application.Helpers;
using EagleEye.Application.Paging;
using MediatR;
using MongoDB.Bson;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Contracts.Persistence;

namespace EagleEye.Application.Features.Queries.GetPageCollectionItems
{
    internal class GetPageCollectionQueryHandler : IRequestHandler<GetPageCollectionQuery, PagedList>
    {
        private readonly ICollectionRepository _repository;

        public GetPageCollectionQueryHandler(ICollectionRepository repository)
            => _repository = repository;

        public async Task<PagedList> Handle(GetPageCollectionQuery request, CancellationToken cancellationToken)
        {
            var data = await _repository.GetAllAsync(request.CollectionName, request.Parameters);
            return new PagedList(JsonHelper.Correction(data.ToJson()),
                                 data.Count,
                                 request.Parameters.PageNumber,
                                 request.Parameters.PageSize);
        }
    }
}
