using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;

public interface IAuctionRepository
{
    Task<Auction> GetById(Guid id);
    Task<Auction> Register(Auction request);
    Task<Auction> Update(Auction request);
    IQueryable<Auction> GetAll();
    Task<int> CountAsync();
}
