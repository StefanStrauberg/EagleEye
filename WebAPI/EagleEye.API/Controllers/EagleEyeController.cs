using System;
using System.Net;
using System.Threading.Tasks;
using EagleEye.Application.Features.Commands.DeleteCollectionItem;
using EagleEye.Application.Features.Commands.UpdateCollectionItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllCollectionItems(string collectionName)
        {
            var data = await _mediator.Send(new GetCollectionQuery(collectionName));
            return Ok(data);
        }

        [HttpGet("{collectionName}/{id}")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCollectionItem(string collectionName, Guid id)
        {
            var data = await _mediator.Send(new GetCollectionItemByIdQuery(collectionName, id));
            return Ok(data);
        }

        [HttpPost("{collectionName}")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateCollectionItem(string collectionName, object item)
        {
            await _mediator.Send(new CreateCollectionItemCommand(collectionName, item));
            return Ok();
        }

        [HttpPut("{collectionName}")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCollectionItem(string collectionName, object item)
        {
            await _mediator.Send(new UpdateCollectionItemCommand(collectionName, item));
            return Ok();
        }

        [HttpDelete("{collectionName}/{id}")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteCollectionItem(string collectionName, Guid id)
        {
            await _mediator.Send(new DeleteCollectionItemCommand(collectionName, id));
            return Ok();
        }
    }
}