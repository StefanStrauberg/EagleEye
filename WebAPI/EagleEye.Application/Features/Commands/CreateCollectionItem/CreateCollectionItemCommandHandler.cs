using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebAPI.EagleEye.Application.Contracts.Persistence;

namespace WebAPI.EagleEye.Application.Features.Commands.CreateCollectionItem
{
    internal class CreateCollectionItemCommandHandler : IRequestHandler<CreateCollectionItemCommand, Unit>
    {
        private readonly ICollectionRepository  _repository;

        public CreateCollectionItemCommandHandler(ICollectionRepository repository)
            => _repository = repository;

        public async Task<Unit> Handle(CreateCollectionItemCommand request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(request.CollectionName, request.Item);
            return Unit.Value;
        }
    }
}