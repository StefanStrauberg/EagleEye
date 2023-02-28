using MediatR;

namespace WebAPI.EagleEye.Application.Features.Commands.UpdateCollectionItem
{
    public record UpdateCollectionItemCommand(string CollectionName, object Item) : IRequest<Unit>;
}