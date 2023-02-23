using MediatR;

namespace FGLogDog.Application.Commands
{
    public record ParseLogCommand(string message) : IRequest<Unit>;
}