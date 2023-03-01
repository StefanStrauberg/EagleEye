using MediatR;
using MongoDB.Bson;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Contracts.Persistence;

namespace WebAPI.EagleEye.Application.Features.Queries.GetAllCollectionItems
{
    internal class GetCollectionQueryHandler : IRequestHandler<GetCollectionQuery, string>
    {
        private readonly ICollectionRepository _repository;

        public GetCollectionQueryHandler(ICollectionRepository repository)
            => _repository = repository;

        public async Task<string> Handle(GetCollectionQuery request, CancellationToken cancellationToken)
        {
            var data = await _repository.GetAllAsync(request.CollectionName);
            var json = data.ToJson();
            var result = Regex.Match(json, @"ObjectId\(([^\)]*)\)").Value;
            var id = result.Replace("ObjectId(", string.Empty).Replace(")", String.Empty);
            var validJson = json.Replace(result, id);
            return validJson;
        }
    }
}