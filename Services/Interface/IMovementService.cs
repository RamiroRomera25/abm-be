using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Dtos.Common;

namespace technical_tests_backend_ssr.Services.Interface;

public interface IMovementService
{
    Task<MovementDto> GetById(Guid id);
    Task<PagedResult<MovementDto>> GetAll(int pageNumber, int pageSize);
    Task<MovementDto> Register(MovementDtoPost request);
    Task<MovementDto> SoftDelete(Guid id);
    Task<MovementDto> Reactivate(Guid id);
}
