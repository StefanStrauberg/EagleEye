using MediatR;
using System;

namespace WebAPI.EagleEye.Application.Features.Queries.GetCollectionItem
{
    public record GetCollectionItemByIdQuery(string CollectionName, Guid Id) : IRequest<string>;
}