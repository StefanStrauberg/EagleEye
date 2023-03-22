using MediatR;

namespace EagleEye.Application.Features.Commands.DeleteCollectionItem
{
    public record DeleteCollectionItemCommand(string CollectionName, string Id) : IRequest<Unit>;
}