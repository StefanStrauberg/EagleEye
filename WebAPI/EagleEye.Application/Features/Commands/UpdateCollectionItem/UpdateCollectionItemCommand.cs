using MediatR;
using System.Text.Json.Nodes;

namespace EagleEye.Application.Features.Commands.UpdateCollectionItem
{
    public record UpdateCollectionItemCommand(string CollectionName, string Id, JsonObject JsonItem) : IRequest<Unit>;
}