using MediatR;
using WebAPI.EagleEye.Application.Models.Paging;
using WebAPI.EagleEye.Application.Models.RequestFeatures;

namespace EagleEye.Application.Features.Queries.GetPageCollectionItems
{
    public record GetPageCollectionQuery(string CollectionName, QueryParameters Parameters) : IRequest<PagedList>;
}
