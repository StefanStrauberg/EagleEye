using MediatR;
using MongoDB.Bson;
using System.Text.Json.Nodes;

namespace EagleEye.Application.Features.Commands.UpdateCollectionItem
{
    public record UpdateCollectionItemCommand(string CollectionName, ObjectId id, JsonObject JsonItem) : IRequest<Unit>;
}