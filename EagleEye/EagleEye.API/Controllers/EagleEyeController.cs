using EagleEye.Application.Exceptions;
using EagleEye.Application.Features.Commands.DeleteCollectionItem;
using EagleEye.Application.Features.Commands.UpdateCollectionItem;
using EagleEye.Application.Features.Queries.GetPageCollectionItems;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Features.Commands.CreateCollectionItem;
using WebAPI.EagleEye.Application.Features.Queries.GetAllCollectionItems;
using WebAPI.EagleEye.Application.Features.Queries.GetCollectionItem;
using WebAPI.EagleEye.Application.Models.Paging;
using WebAPI.EagleEye.Application.Models.RequestFeatures;

namespace WebAPI.EagleEye.API.Controllers
{
    public class EagleEyeController : BaseController
    {
        private readonly IMediator _mediator;

        public EagleEyeController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("paged/{collectionName}")]
        [ProducesResponseType(typeof(JsonObject), (int)HttpStatusCode.OK, "application/json")]
        public async Task<IActionResult> GetPagedCollectionItems(string collectionName, [FromQuery] QueryParameters parameters)
        {
            PagedList result = await _mediator.Send(new GetPageCollectionQuery(collectionName, parameters));
            Response.Headers.Add("X-Pagination", result.MetaData.ToString());
            return Content(result.data, "application/json");
        }

        [HttpGet("{collectionName}")]
        [ProducesResponseType(typeof(JsonObject), (int)HttpStatusCode.OK, "application/json")]
        public async Task<IActionResult> GetAllCollectionItems(string collectionName)
            => Content(await _mediator.Send(new GetCollectionQuery(collectionName)), "application/json");

        [HttpGet("{collectionName}/{id}")]
        [ProducesResponseType(typeof(JsonObject), (int)HttpStatusCode.OK, "application/json")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound, "application/json")]
        public async Task<IActionResult> GetCollectionItem(string collectionName, string id)
            => Content(await _mediator.Send(new GetCollectionItemByIdQuery(collectionName, id)), "application/json");

        [HttpPost("{collectionName}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateCollectionItem(string collectionName, JsonObject JsonItem)
            => Ok(await _mediator.Send(new CreateCollectionItemCommand(collectionName, JsonItem)));

        [HttpPut("{collectionName}/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound, "application/json")]
        public async Task<IActionResult> UpdateCollectionItem(string collectionName, string id, JsonObject JsonItem)
            => Ok(await _mediator.Send(new UpdateCollectionItemCommand(collectionName, id, JsonItem)));

        [HttpDelete("{collectionName}/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound, "application/json")]
        public async Task<IActionResult> DeleteCollectionItem(string collectionName, string id)
            => Ok(await _mediator.Send(new DeleteCollectionItemCommand(collectionName, id)));
    }
}