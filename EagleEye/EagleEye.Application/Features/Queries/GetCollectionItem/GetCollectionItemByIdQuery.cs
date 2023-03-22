using MediatR;

namespace WebAPI.EagleEye.Application.Features.Queries.GetCollectionItem
{
    public record GetCollectionItemByIdQuery(string CollectionName, string Id) : IRequest<string>;
}