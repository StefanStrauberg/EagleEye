using MediatR;

namespace FGLogDog.Application.Queries
{
    public record ParseFGLogQuery(string message) : IRequest<Unit>;
}