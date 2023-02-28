using MediatR;

namespace WebAPI.EagleEye.Application.Features.Commands.CreateCollectionItem
{
    public record CreateCollectionItemCommand(string CollectionName, object Item) : IRequest<Unit>;
}