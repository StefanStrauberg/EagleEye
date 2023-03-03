using EagleEye.Application.Helpers;
using MediatR;
using MongoDB.Bson;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Contracts.Persistence;
using WebAPI.EagleEye.Application.Models.Paging;

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
            return new PagedList(jsonString: JsonHelper.Correction(data.ToJson()),
                                 countGetItems: data.items.Count,
                                 countOfItemsByFilter: data.countOfItemsByFilter,
                                 pageNumber: request.Parameters.PageNumber,
                                 pageSize: request.Parameters.PageSize);
        }
    }
}
