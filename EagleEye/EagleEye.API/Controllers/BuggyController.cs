using EagleEye.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Features.Queries.GetCollectionItem;

namespace WebAPI.EagleEye.API.Controllers
{
    public class BuggyController : BaseController
    {
        private readonly IMediator _mediator;

        public BuggyController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("NotFound")]
        public async Task<IActionResult> GetCollectionItem()
            => Content(await _mediator.Send(new GetCollectionItemByIdQuery("buggy", "123")), "application/json");

        [HttpGet("ServerError")]
        public async Task<IActionResult> GetServerError()
        {
            var thing = new { };
            thing = null;
            var thingToReturn = thing.ToString();
            return await Task.Run(() => Ok());
        }

        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
    }
}