using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Common;

namespace NetCoreApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult HandleResult<T>(ServiceResult<T> result)
    {
        if (result.Success)
        {
            return result.StatusCode switch
            {
                201 => StatusCode(StatusCodes.Status201Created, result),
                _ => Ok(result)
            };
        }

        return result.StatusCode switch
        {
            400 => BadRequest(result),
            401 => Unauthorized(result),
            403 => Forbid(),
            404 => NotFound(result),
            409 => Conflict(result),
            _ => StatusCode(result.StatusCode, result)
        };
    }
}
