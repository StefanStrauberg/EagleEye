using MediatR;
using System.Text.Json.Nodes;

namespace WebAPI.EagleEye.Application.Features.Commands.CreateCollectionItem
{
    public record CreateCollectionItemCommand(string CollectionName, JsonObject JsonItem) : IRequest<Unit>;
}