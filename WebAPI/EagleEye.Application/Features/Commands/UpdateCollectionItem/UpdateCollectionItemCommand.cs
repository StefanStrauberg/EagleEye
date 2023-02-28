using MediatR;
using System.Text.Json.Nodes;

namespace EagleEye.Application.Features.Commands.UpdateCollectionItem
{
    public record UpdateCollectionItemCommand(string CollectionName, JsonObject JsonItem) : IRequest<Unit>;
}