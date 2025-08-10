using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Domain;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories.Impl;

public class AuctionRepository : IAuctionRepository
{
    
    private readonly TechnicalTestDbContext _context;

    public AuctionRepository(TechnicalTestDbContext context)
    {
        _context = context;
    }
    
    public async Task<Auction> GetById(Guid id)
    {
        return await _context.Auctions.FindAsync(id);
    }

    public IQueryable<Auction> GetAll()
    {
        return _context.Auctions.AsNoTracking();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Auctions.CountAsync();
    }

    public async Task<Auction> Register(Auction request)
    {
        await _context.Auctions.AddAsync(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<Auction> Update(Auction request)
    {
        _context.Update(request);
        await _context.SaveChangesAsync();
        return request;
    }
}