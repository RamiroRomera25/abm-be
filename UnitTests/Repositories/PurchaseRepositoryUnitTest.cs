using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Domain;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using Xunit;

namespace technical_tests_backend_ssr.UnitTests.Repositories;

public class PurchaseRepositoryUnitTest
{
    private TechnicalTestDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TechnicalTestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TechnicalTestDbContext(options);
    }

    [Fact]
    public async Task GetById_ReturnsEntity_WhenExists()
    {
        using var context = CreateContext();
        var repo = new PurchaseRepository(context);
        var id = Guid.NewGuid();
        var purchase = new Purchase { PurchaseId = id, Title = "Test Purchase" };

        context.Purchases.Add(purchase);
        await context.SaveChangesAsync();

        var result = await repo.GetById(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.PurchaseId);
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenNotExists()
    {
        using var context = CreateContext();
        var repo = new PurchaseRepository(context);

        var result = await repo.GetById(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_ReturnsAllPurchases()
    {
        using var context = CreateContext();
        var repo = new PurchaseRepository(context);

        context.Purchases.AddRange(
            new Purchase { PurchaseId = Guid.NewGuid(), Title = "P1" },
            new Purchase { PurchaseId = Guid.NewGuid(), Title = "P2" }
        );
        await context.SaveChangesAsync();

        var results = repo.GetAll().ToList();

        Assert.Equal(2, results.Count);
    }

    [Fact]
    public async Task CountAsync_ReturnsCorrectCount()
    {
        using var context = CreateContext();
        var repo = new PurchaseRepository(context);

        context.Purchases.AddRange(
            new Purchase { PurchaseId = Guid.NewGuid(), Title = "P1" },
            new Purchase { PurchaseId = Guid.NewGuid(), Title = "P2" }
        );
        await context.SaveChangesAsync();

        var count = await repo.CountAsync();

        Assert.Equal(2, count);
    }

    [Fact]
    public async Task Register_AddsPurchase()
    {
        using var context = CreateContext();
        var repo = new PurchaseRepository(context);
        var purchase = new Purchase { PurchaseId = Guid.NewGuid(), Title = "New Purchase" };

        var result = await repo.Register(purchase);

        Assert.NotNull(result);
        Assert.Equal("New Purchase", result.Title);
        Assert.Equal(1, await context.Purchases.CountAsync());
    }

    [Fact]
    public async Task Update_ModifiesExistingPurchase()
    {
        using var context = CreateContext();
        var repo = new PurchaseRepository(context);
        var id = Guid.NewGuid();
        var purchase = new Purchase { PurchaseId = id, Title = "Old Name" };

        context.Purchases.Add(purchase);
        await context.SaveChangesAsync();

        purchase.Title = "Updated Name";
        var result = await repo.Update(purchase);

        Assert.Equal("Updated Name", result.Title);
        Assert.Equal("Updated Name", (await context.Purchases.FindAsync(id)).Title);
    }
}