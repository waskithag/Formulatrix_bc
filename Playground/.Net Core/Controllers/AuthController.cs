using Microsoft.AspNetCore.Mvc;
using NetCoreApp.DTOs;
using NetCoreApp.Services;

namespace NetCoreApp.Controllers;

public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        var result = _authService.Authenticate(request);
        return HandleResult(result);
    }
}
