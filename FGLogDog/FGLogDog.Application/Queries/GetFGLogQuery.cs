using MediatR;

namespace FGLogDog.Application.Queries
{
    public record GetFGLogQuery(string inputLine) : IRequest<Unit>;
}