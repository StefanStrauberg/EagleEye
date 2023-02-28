using MediatR;
using System.Collections.Generic;

namespace WebAPI.EagleEye.Application.Features.Queries.GetAllCollectionItems
{
    public record GetCollectionQuery(string CollectionName) : IRequest<IReadOnlyList<string>>;
}