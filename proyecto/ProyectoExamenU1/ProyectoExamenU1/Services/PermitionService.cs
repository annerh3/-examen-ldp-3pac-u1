using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoExamenU1.Constants;
using ProyectoExamenU1.Database;
using ProyectoExamenU1.Database.Entities;
using ProyectoExamenU1.Dtos.Common;
using ProyectoExamenU1.Dtos.Permitions;
using ProyectoExamenU1.Services.Interfaces;
using System.Security.Claims;

namespace ProyectoExamenU1.Services
{
    public class PermitionService : IPermitionApplicationService
    {
        private readonly ProyectoExamenContext _context;
        private readonly IAuditService _auditService;
        private readonly ILogger<IPermitionApplicationService> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly int PAGE_SIZE;
        public PermitionService(
            ProyectoExamenContext context,
            IAuditService auditService,
            ILogger<IPermitionApplicationService> logger,
            IMapper mapper,
            IConfiguration configuration,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            this._context = context;
            this._auditService = auditService;
            this._logger = logger;
            this._mapper = mapper;
            this._configuration = configuration;
            this._userManager = userManager;
            this._roleManager = roleManager;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
        }
        public async Task<ResponseDto<ApplicationPermitionDto>> CreatePermitionAsync(ApplicationPerrmitionCreateDto dto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var permitionEntity = _mapper.Map<PermitionApplicationEntity>(dto);

                    Guid typeId = permitionEntity.PermitionTypeId;

                    var exist = await _context.PermitionTypes.FirstOrDefaultAsync(e => e.Id == typeId);


                    if (exist == null)
                    {
                        throw new Exception("La tipo de permiso no existe.");
                    }

                    if (dto.StartDate < dto.EndDate)
                    {
                        throw new Exception("La fecha no puede ser menos a la de salids");
                    }

                     
                    int days = (dto.EndDate - dto.StartDate).Days;

                    if (days > exist.MaxDays)
                    {
                        throw new Exception("No se puede pedir mas dias del maximo permitido por ley");
                    }

                    _context.ApplicationEntities.Add(permitionEntity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var createdPermitionDto = _mapper.Map<ApplicationPermitionDto>(permitionEntity);
                    return new ResponseDto<ApplicationPermitionDto>
                    {
                        StatusCode = 201,
                        Status = true,
                        Message = MessagesConstant.CREATE_SUCCESS,
                        Data = createdPermitionDto
                    };
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(e, MessagesConstant.CREATE_ERROR);

                    // Retorna una respuesta de error
                    return new ResponseDto<ApplicationPermitionDto>
                    {
                        StatusCode = 500,
                        Status = false,
                        Message = MessagesConstant.CREATE_ERROR
                    };
                }
            }
        }

        public async Task<ResponseDto<ApplicationPermitionDto>> DeleteAsync(Guid id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var permitionEntity = await _context.ApplicationEntities.FindAsync(id);

                    if (permitionEntity is null)
                    {
                        return new ResponseDto<ApplicationPermitionDto>
                        {
                            StatusCode = 404,
                            Status = false,
                            Message = $"{MessagesConstant.RECORD_NOT_FOUND} {id}"
                        };
                    }

                    _context.ApplicationEntities.Remove(permitionEntity);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                        
                    return new ResponseDto<ApplicationPermitionDto>
                    {
                        StatusCode = 200,
                        Status = true,
                        Message = MessagesConstant.DELETE_SUCCESS
                    };
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(e, MessagesConstant.DELETE_ERROR);
                    return new ResponseDto<ApplicationPermitionDto>
                    {
                        StatusCode = 500,
                        Status = false,
                        Message = MessagesConstant.DELETE_ERROR
                    };
                }
            }
        }

        public async Task<ResponseDto<ApplicationPermitionDto>> EditAsync(ApplicationPermitionEditDto dto, Guid id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var permitionEntity = await _context.ApplicationEntities.FindAsync(id);

                    if (permitionEntity is null)
                    {
                        return new ResponseDto<ApplicationPermitionDto>
                        {
                            StatusCode = 404,
                            Status = false,
                            Message = $"{MessagesConstant.RECORD_NOT_FOUND} {id}"
                        };
                    }

                    _mapper.Map(dto, permitionEntity);
                    //postEntity.AuthorId = _authService.GetUserId(); //TODO: Remover cuanto este el frontend con nueva logica

                    _context.ApplicationEntities.Update(permitionEntity);
                    await _context.SaveChangesAsync();

                    //throw new Exception("Error para validar el rollback.");

                    await transaction.CommitAsync();

                    return new ResponseDto<ApplicationPermitionDto>
                    {
                        StatusCode = 200,
                        Status = true,
                        Message = MessagesConstant.UPDATE_SUCCESS
                    };

                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(e, MessagesConstant.UPDATE_ERROR);
                    return new ResponseDto<ApplicationPermitionDto>
                    {
                        StatusCode = 500,
                        Status = false,
                        Message = MessagesConstant.UPDATE_ERROR
                    };
                }
            }
        }

        public async Task<ResponseDto<PaginationDto<List<ApplicationPermitionDto>>>> GetPermitionsListAsync( string searchTerm = "", int page = 1)
        {

            int startIndex = (page - 1) * PAGE_SIZE;

            var userId = _auditService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            var isEmployee = await _userManager.IsInRoleAsync(user, RolesConstant.EMPLOYEE);
            var permitionsEntityQuery = _context.ApplicationEntities.AsQueryable();
            if (!isEmployee)
            {
                permitionsEntityQuery = _context.ApplicationEntities
                 .Where(x => x.Reason.ToLower().Contains(searchTerm.ToLower()));
            }
            else
            {
                permitionsEntityQuery = _context.ApplicationEntities
                .Where(x => x.CreatedBy == userId && (string.IsNullOrEmpty(searchTerm) || x.Reason.ToLower().Contains(searchTerm.ToLower())));
            }


            int totalPermitions = await permitionsEntityQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalPermitions / PAGE_SIZE);

            var permitionEntity = await permitionsEntityQuery
                .OrderBy(u => u.Reason)
                .Skip(startIndex)
                .Take(PAGE_SIZE)
                .ToListAsync();

            var permitionsDtos = _mapper.Map<List<ApplicationPermitionDto>>(permitionEntity);

            return new ResponseDto<PaginationDto<List<ApplicationPermitionDto>>>
            {
                StatusCode = 200,
                Status = true,
                Message = MessagesConstant.RECORDS_FOUND,
                Data = new PaginationDto<List<ApplicationPermitionDto>>
                {
                    CurrentPage = page,
                    PageSize = PAGE_SIZE,
                    TotalItems = totalPermitions,
                    TotalPages = totalPages,
                    Items = permitionsDtos,
                    HasPreviousPage = page > 1,
                    HasNextPage = page < totalPages,
                }
            };
            
            }

        public async Task<ResponseDto<ApplicationPermitionDto>> GetPermitionByIdAsync(Guid id)
        {
            var postEntity = await _context.ApplicationEntities
                .FirstOrDefaultAsync(x => x.Id == id);

                        if (postEntity is null)
                        {
                            return new ResponseDto<ApplicationPermitionDto>
                            {
                                StatusCode = 404,
                                Status = false,
                                Message = $"{MessagesConstant.RECORD_NOT_FOUND} {id}"
                            };
                        }

                        var postDto = _mapper.Map<ApplicationPermitionDto>(postEntity);

                        return new ResponseDto<ApplicationPermitionDto>
                        {
                            StatusCode = 200,
                            Status = true,
                            Message = MessagesConstant.RECORD_FOUND,
                            Data = postDto,
                        };
        }
    }
}
