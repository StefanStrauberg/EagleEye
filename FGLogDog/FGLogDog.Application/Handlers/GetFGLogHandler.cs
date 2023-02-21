using System.Threading;
using System.Threading.Tasks;
using FGLogDog.Application.Queries;
using FGLogDog.Domain;
using MediatR;

namespace FGLogDog.Application.Handlers
{
    public class GetFGLogHandler : IRequestHandler<GetFGLogQuery, FortigateLog>
    {
        
        public Task<FortigateLog> Handle(GetFGLogQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}