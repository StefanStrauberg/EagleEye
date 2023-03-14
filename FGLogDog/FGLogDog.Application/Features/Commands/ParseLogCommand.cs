using MediatR;

namespace FGLogDog.Application.Features.Commands
{
    public record ParseLogCommand(string inputLog) : IRequest<Unit>;
}