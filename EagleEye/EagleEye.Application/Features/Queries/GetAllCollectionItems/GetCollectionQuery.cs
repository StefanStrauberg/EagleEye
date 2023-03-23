using MediatR;

namespace WebAPI.EagleEye.Application.Features.Queries.GetAllCollectionItems
{
    public record GetCollectionQuery(string CollectionName) : IRequest<string>;
}