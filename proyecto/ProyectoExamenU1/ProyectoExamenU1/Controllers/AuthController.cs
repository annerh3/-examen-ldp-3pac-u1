using BlogUNAH.API.Dtos.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoExamenU1.Constants;
using ProyectoExamenU1.Dtos.Common;
using ProyectoExamenU1.Services.Interfaces;

namespace BlogUNAH.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService
            )
        {
            this._authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<LoginResponseDto>>> Login(LoginDto dto) 
        {
            var response = await _authService.LoginAsync(dto);
            return StatusCode(response.StatusCode, response);
        }


        [HttpDelete("delete/{id}")]
        //[Authorize(Roles = $"{RolesConstant.ADMIN}, {RolesConstant.HUMAN_RESOURCES}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<IdentityUser>>> Delete(Guid id)
        {
            var response = await _authService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
