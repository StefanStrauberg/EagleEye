using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebAPI.EagleEye.Application.Contracts.Persistence;

namespace WebAPI.EagleEye.Application.Features.Commands.UpdateCollectionItem
{
    internal class UpdateCollectionItemCommandHandler : IRequestHandler<UpdateCollectionItemCommand, Unit>
    {
        private readonly ICollectionRepository _repository;

        public UpdateCollectionItemCommandHandler(ICollectionRepository repository)
            => _repository = repository;

        public async Task<Unit> Handle(UpdateCollectionItemCommand request, CancellationToken cancellationToken)
        {
            await _repository.UpdateAsync(request.CollectionName, request.Item);
            return Unit.Value;
        }
    }
}