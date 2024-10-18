using ProyectoExamenU1.Dtos.Common;
using ProyectoExamenU1.Dtos.Permitions;

namespace ProyectoExamenU1.Services.Interfaces
{
    public interface IPermitionApplicationService
    {
        Task<ResponseDto<PaginationDto<List<ApplicationPermitionDto>>>> GetPermitionsListAsync( string searchTerm = "", int page = 1 );
        Task<ResponseDto<ApplicationPermitionDto>> GetPermitionByIdAsync(Guid id);
        Task<ResponseDto<ApplicationPermitionDto>> CreatePermitionAsync(ApplicationPerrmitionCreateDto dto);
        Task<ResponseDto<ApplicationPermitionDto>> EditAsync(ApplicationPermitionEditDto dto, Guid id);
        Task<ResponseDto<ApplicationPermitionDto>> DeleteAsync(Guid id);
    }
}
