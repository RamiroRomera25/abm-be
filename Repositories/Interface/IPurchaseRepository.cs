using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;

public interface IPurchaseRepository
{
    Task<Purchase> GetById(Guid id);
    Task<Purchase> Register(Purchase request);
    Task<Purchase> Update(Purchase request);
    IQueryable<Purchase> GetAll();
    Task<int> CountAsync();
}