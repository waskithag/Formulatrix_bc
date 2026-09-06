using NetCoreApp.Common;
using NetCoreApp.DTOs;

namespace NetCoreApp.Services;

public interface IAuthService
{
    ServiceResult<LoginResponseDto> Authenticate(LoginRequestDto request);
}
