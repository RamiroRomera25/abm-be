using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Dtos.Common;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Services.Interface;

public interface IPurchaseService
{
    Task<PurchaseDto> GetById(Guid id);
    Task<Purchase> GetModelById(Guid id);
    Task<PagedResult<PurchaseDto>> GetAll(int pageNumber, int pageSize);
    Task<PurchaseDto> Register(PurchaseDtoPost request);
    Task<PurchaseDto> Update(PurchaseDtoPut request);
    Task<PurchaseDto> SoftDelete(Guid id);
    Task<PurchaseDto> Reactivate(Guid id);
    Task UpdateCurrentAmount(Movement movement);
}
