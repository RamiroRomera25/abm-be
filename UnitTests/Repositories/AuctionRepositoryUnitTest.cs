using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Domain;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories.Impl;
using Xunit;

namespace technical_tests_backend_ssr.UnitTests.Repositories;

public class AuctionRepositoryUnitTest
{
    private TechnicalTestDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TechnicalTestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Base de datos aislada por test
            .Options;

        return new TechnicalTestDbContext(options);
    }

    [Fact]
    public async Task GetById_ReturnsEntity_WhenExists()
    {
        using var context = CreateContext();
        var repo = new AuctionRepository(context);
        var id = Guid.NewGuid();
        var auction = new Auction { AuctionId = id, Title = "Test Auction" };

        context.Auctions.Add(auction);
        await context.SaveChangesAsync();

        var result = await repo.GetById(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.AuctionId);
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenNotExists()
    {
        using var context = CreateContext();
        var repo = new AuctionRepository(context);

        var result = await repo.GetById(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_ReturnsAllAuctions()
    {
        using var context = CreateContext();
        var repo = new AuctionRepository(context);

        context.Auctions.AddRange(
            new Auction { AuctionId = Guid.NewGuid(), Title = "A1" },
            new Auction { AuctionId = Guid.NewGuid(), Title = "A2" }
        );
        await context.SaveChangesAsync();

        var results = repo.GetAll().ToList();

        Assert.Equal(2, results.Count);
    }

    [Fact]
    public async Task CountAsync_ReturnsCorrectCount()
    {
        using var context = CreateContext();
        var repo = new AuctionRepository(context);

        context.Auctions.AddRange(
            new Auction { AuctionId = Guid.NewGuid(), Title = "A1" },
            new Auction { AuctionId = Guid.NewGuid(), Title = "A2" }
        );
        await context.SaveChangesAsync();

        var count = await repo.CountAsync();

        Assert.Equal(2, count);
    }

    [Fact]
    public async Task Register_AddsAuction()
    {
        using var context = CreateContext();
        var repo = new AuctionRepository(context);
        var auction = new Auction { AuctionId = Guid.NewGuid(), Title = "New Auction" };

        var result = await repo.Register(auction);

        Assert.NotNull(result);
        Assert.Equal("New Auction", result.Title);
        Assert.Equal(1, await context.Auctions.CountAsync());
    }

    [Fact]
    public async Task Update_ModifiesExistingAuction()
    {
        using var context = CreateContext();
        var repo = new AuctionRepository(context);
        var id = Guid.NewGuid();
        var auction = new Auction { AuctionId = id, Title = "Old Name" };

        context.Auctions.Add(auction);
        await context.SaveChangesAsync();

        auction.Title = "Updated Name";
        var result = await repo.Update(auction);

        Assert.Equal("Updated Name", result.Title);
        Assert.Equal("Updated Name", (await context.Auctions.FindAsync(id)).Title);
    }
}