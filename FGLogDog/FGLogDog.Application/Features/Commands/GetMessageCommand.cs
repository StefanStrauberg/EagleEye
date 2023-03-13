using System.Text.Json.Nodes;
using MediatR;

namespace FGLogDog.Application.Features.Commands
{
    internal record GetMessageCommand : IRequest<JsonObject>;
}