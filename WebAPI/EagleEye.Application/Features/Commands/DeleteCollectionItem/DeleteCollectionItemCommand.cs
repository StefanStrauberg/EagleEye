using MediatR;
using MongoDB.Bson;

namespace EagleEye.Application.Features.Commands.DeleteCollectionItem
{
    public record DeleteCollectionItemCommand(string CollectionName, ObjectId Id) : IRequest<Unit>;
}