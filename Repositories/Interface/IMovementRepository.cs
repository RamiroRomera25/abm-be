using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;

public interface IMovementRepository
{
    
    Task<Movement> GetById(Guid id);
    Task<Movement> Register(Movement request);
    Task<Movement> Update(Movement request);
    IQueryable<Movement> GetAll();
    Task<int> CountAsync();
}