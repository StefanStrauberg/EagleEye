using MediatR;

namespace FGLogDog.Application.Commands
{
    internal record ParseLogCommand(string inputLog) : IRequest<Unit>;
}