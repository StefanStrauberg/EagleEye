using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebAPI.EagleEye.Application.Contracts.Persistence;

namespace EagleEye.Application.Features.Commands.DeleteCollectionItem
{
    public class DeleteCollectionItemCommandHandler : IRequestHandler<DeleteCollectionItemCommand, Unit>
    {
        private readonly ICollectionRepository _repository;

        public DeleteCollectionItemCommandHandler(ICollectionRepository repository)
            => _repository = repository;

        public async Task<Unit> Handle(DeleteCollectionItemCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.CollectionName, request.Id);
            return Unit.Value;
        }
    }
}