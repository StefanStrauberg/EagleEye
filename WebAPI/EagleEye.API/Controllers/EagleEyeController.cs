using EagleEye.Application.Features.Commands.DeleteCollectionItem;
using EagleEye.Application.Features.Commands.UpdateCollectionItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Features.Commands.CreateCollectionItem;
using WebAPI.EagleEye.Application.Features.Queries.GetAllCollectionItems;
using WebAPI.EagleEye.Application.Features.Queries.GetCollectionItem;

namespace WebAPI.EagleEye.API.Controllers
{
    public class EagleEyeController : BaseController
    {
        private readonly IMediator _mediator;

        public EagleEyeController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("{collectionName}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllCollectionItems(string collectionName)
            => Ok(await _mediator.Send(new GetCollectionQuery(collectionName)));

        [HttpGet("{collectionName}/{id}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCollectionItem(string collectionName, ObjectId id)
            => Ok(await _mediator.Send(new GetCollectionItemByIdQuery(collectionName, id)));

        [HttpPost("{collectionName}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateCollectionItem(string collectionName, JsonObject JsonItem)
            => Ok(await _mediator.Send(new CreateCollectionItemCommand(collectionName, JsonItem)));

        [HttpPut("{collectionName}/{id}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCollectionItem(string collectionName, ObjectId id, JsonObject JsonItem)
            => Ok(await _mediator.Send(new UpdateCollectionItemCommand(collectionName, id, JsonItem)));

        [HttpDelete("{collectionName}/{id}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteCollectionItem(string collectionName, ObjectId id)
            => Ok(await _mediator.Send(new DeleteCollectionItemCommand(collectionName, id)));
    }
}