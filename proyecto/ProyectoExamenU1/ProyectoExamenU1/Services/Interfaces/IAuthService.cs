using BlogUNAH.API.Dtos.Auth;
using Microsoft.AspNetCore.Identity;
using ProyectoExamenU1.Dtos.Common;

namespace ProyectoExamenU1.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto dto);
        Task<ResponseDto<IdentityUser>> DeleteAsync(Guid id);
    }
}
