using Azure.Core;
using LibraryMAnagementSystem.API.Errors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LibraryMAnagementSystem.API.Controllers.Common
{
    [ApiController]
    [Route("Errors/{Code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Error(int Code)
        {

            if (Code == (int)HttpStatusCode.NotFound)
            {
                var res = new ApiResponse((int)HttpStatusCode.NotFound, message: $"The Requested End Point {Request.Path}  is not Found");

                return NotFound(res);
            }
            else if (Code == (int)HttpStatusCode.BadRequest)
            {
                var res = new ApiResponse((int)HttpStatusCode.BadRequest, message: $"The Request is Bad Request");

                return BadRequest(res);
            }
            else if (Code == (int)HttpStatusCode.Unauthorized)
            {
                var res = new ApiResponse((int)HttpStatusCode.Unauthorized, message: $"The Request is UnAuthorized");

                return BadRequest(res);
            }
            return StatusCode(Code, new ApiResponse(Code));
        }
    }
}
