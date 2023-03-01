using EagleEye.Application.Paging;
using EagleEye.Application.RequestFeatures;
using MediatR;

namespace EagleEye.Application.Features.Queries.GetPageCollectionItems
{
    public record GetPageCollectionQuery(string CollectionName, QueryParameters Parameters) : IRequest<PagedList>;
}
