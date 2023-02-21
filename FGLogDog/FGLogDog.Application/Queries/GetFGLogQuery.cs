using FGLogDog.Domain;
using MediatR;

namespace FGLogDog.Application.Queries
{
    public record GetFGLogQuery(string input) : IRequest<FortigateLog>;
}