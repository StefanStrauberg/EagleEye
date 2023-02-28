using System;
using MediatR;

namespace WebAPI.EagleEye.Application.Features.Commands.DeleteCollectionItem
{
    public record DeleteCollectionItemCommand(string CollectionName, Guid Id) : IRequest<Unit>;
}