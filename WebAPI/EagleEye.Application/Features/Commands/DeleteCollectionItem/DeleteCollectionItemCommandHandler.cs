using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Contracts.Persistence;
using WebAPI.EagleEye.Application.Exceptions;

namespace EagleEye.Application.Features.Commands.DeleteCollectionItem
{
    public class DeleteCollectionItemCommandHandler : IRequestHandler<DeleteCollectionItemCommand, Unit>
    {
        private readonly ICollectionRepository _repository;

        public DeleteCollectionItemCommandHandler(ICollectionRepository repository)
            => _repository = repository;

        public async Task<Unit> Handle(DeleteCollectionItemCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.DeleteAsync(request.CollectionName, request.Id))
                return Unit.Value;
            throw new NotFoundException(request.CollectionName, request.Id);
        }
    }
}