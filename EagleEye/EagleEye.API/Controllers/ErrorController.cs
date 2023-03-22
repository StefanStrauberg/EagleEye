using EagleEye.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.EagleEye.API.Controllers;

namespace EagleEye.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("errors/{code}")]
    public class ErrorController : BaseController
    {
        public async Task<IActionResult> Error(int code)
            => await Task.Run(() => new ObjectResult(new ApiResponse(code)));
    }
}
