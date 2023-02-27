using MediatR;

namespace FGLogDog.Application.Commands
{
    public record ParseLogCommand(string inputLog) : IRequest<Unit>;
}