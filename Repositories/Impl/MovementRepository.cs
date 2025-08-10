using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Domain;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;

public class MovementRepository : IMovementRepository
{
    private readonly TechnicalTestDbContext _context;

    public MovementRepository(TechnicalTestDbContext context)
    {
        _context = context;
    }
    
    public async Task<Movement> GetById(Guid id)
    {
        return await _context.Movements.FindAsync(id);
    }

    public IQueryable<Movement> GetAll()
    {
        return _context.Movements.AsNoTracking();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Movements.CountAsync();
    }

    public async Task<Movement> Register(Movement request)
    {
        await _context.Movements.AddAsync(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<Movement> Update(Movement request)
    {
        _context.Update(request);
        await _context.SaveChangesAsync();
        return request;
    }
}