using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.EagleEye.Application.Features.Queries.GetAllCollectionItems;

namespace WebAPI.EagleEye.API.Controllers
{
    public class EagleEyeController : BaseController
    {
        private readonly IMediator _mediator;

        public EagleEyeController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("{collectionName}")]
        public async Task<IActionResult> GetAllCollectionItems(string collectionName)
        {
            var data = await _mediator.Send(new GetCollectionQuery(collectionName));
            return await Task.Run(() => Ok());
        }
    }
}