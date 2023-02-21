using MediatR;

namespace FGLogDog.Application.Queries
{
    public record GetFGLogQuery : IRequest<string>;
}