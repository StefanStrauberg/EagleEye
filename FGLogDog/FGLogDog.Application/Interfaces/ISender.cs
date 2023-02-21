using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace FGLogDog.Application.Interfaces
{
    public interface ISender
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
        Task<object> Send(object request, CancellationToken cancellationToken = default);
    }
}