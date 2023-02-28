using System;
using MediatR;

namespace EagleEye.Application.Features.Commands.DeleteCollectionItem
{
    public record DeleteCollectionItemCommand(string CollectionName, Guid Id) : IRequest<Unit>;
}