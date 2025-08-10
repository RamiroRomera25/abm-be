using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Domain;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    
    private readonly TechnicalTestDbContext _context;

    public PurchaseRepository(TechnicalTestDbContext context)
    {
        _context = context;
    }
    
    public async Task<Purchase> GetById(Guid id)
    {
        return await _context.Purchases.FindAsync(id);
    }
    
    public IQueryable<Purchase> GetAll()
    {
        return _context.Purchases.AsNoTracking();
    }
    
    public async Task<int> CountAsync()
    {
        return await _context.Purchases.CountAsync();
    }

    public async Task<Purchase> Register(Purchase request)
    {
        await _context.Purchases.AddAsync(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<Purchase> Update(Purchase request)
    {
        _context.Update(request);
        await _context.SaveChangesAsync();
        return request;
    }
}