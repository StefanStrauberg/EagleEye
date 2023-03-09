using MediatR;

namespace FGLogDog.Application.Features.Commands
{
    internal record ParseLogCommand(string inputLog) : IRequest<Unit>;
}