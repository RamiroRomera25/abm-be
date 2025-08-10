using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Dtos.Common;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Services.Interface;

public interface IAuctionService
{
    Task<AuctionDto> GetById(Guid id);
    Task<Auction> GetModelById(Guid id);
    Task<PagedResult<AuctionDto>> GetAll(int pageNumber, int pageSize);
    Task<AuctionDto> Register(AuctionDtoPost request);
    Task<AuctionDto> Update(AuctionDtoPut request);
    Task<AuctionDto> SoftDelete(Guid id);
    Task<AuctionDto> Reactivate(Guid id);
    Task UpdateHighestBid(Movement movement);
}
