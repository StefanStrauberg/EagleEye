using MediatR;
using MongoDB.Bson;

namespace FGLogDog.Application.Features.Commands
{
    public record GetMessageCommand : IRequest<BsonDocument>;
}