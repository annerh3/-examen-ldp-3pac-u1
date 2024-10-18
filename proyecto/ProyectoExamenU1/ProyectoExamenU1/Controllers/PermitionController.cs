using BlogUNAH.API.Dtos.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoExamenU1.Constants;
using ProyectoExamenU1.Dtos.Common;
using ProyectoExamenU1.Dtos.Permitions;
using ProyectoExamenU1.Services.Interfaces;

namespace ProyectoExamenU1.Controllers
{
    [Route("api/permition")]
    [ApiController]
    public class PermitionController : Controller
    {
        private readonly IPermitionApplicationService _permitionApplication;

        public PermitionController(
            IPermitionApplicationService permitionApplication
            )
        {
            this._permitionApplication = permitionApplication;
        }

        [HttpGet]

        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<List<ApplicationPermitionDto>>>> GetAll(
          string searchTerm = "",
          int page = 1)
        {
            var response = await _permitionApplication.GetPermitionsListAsync(searchTerm, page);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<ApplicationPermitionDto>>> Get(Guid id)
        {
            var response = await _permitionApplication.GetPermitionByIdAsync(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [AllowAnonymous]
        //[Authorize(Roles = "ADMIN, AUTHOR")]
       // [Authorize(Roles = $"{RolesConstant.ADMIN}, {RolesConstant.HUMAN_RESOURCES}")]
        public async Task<ActionResult<ResponseDto<ApplicationPermitionDto>>> Create(ApplicationPerrmitionCreateDto dto)
        {
            var response = await _permitionApplication.CreatePermitionAsync(dto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RolesConstant.ADMIN}, {RolesConstant.HUMAN_RESOURCES}")]
        public async Task<ActionResult<ResponseDto<ApplicationPermitionDto>>> Edit(ApplicationPermitionEditDto dto, Guid id)
        {
            var response = await _permitionApplication.EditAsync(dto, id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RolesConstant.ADMIN}, {RolesConstant.HUMAN_RESOURCES}")]

        public async Task<ActionResult<ResponseDto<ApplicationPermitionDto>>> Delete(Guid id)
        {
            var response = await _permitionApplication.DeleteAsync(id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
