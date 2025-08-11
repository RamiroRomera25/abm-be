using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Domain;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Models.Enums;
using technical_tests_backend_ssr.Repositories;
using Xunit;

namespace technical_tests_backend_ssr.UnitTests.Repositories;

public class MovementRepositoryUnitTest : IDisposable
{
    private readonly List<TechnicalTestDbContext> _contexts = new();
    
    private TechnicalTestDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TechnicalTestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new TechnicalTestDbContext(options);
        _contexts.Add(context);
        return context;
    }

    public void Dispose()
    {
        foreach (var context in _contexts)
        {
            context?.Dispose();
        }
    }

    [Fact]
    public async Task GetById_ReturnsEntity_WhenExists()
    {
        using var context = CreateContext();
        var repo = new MovementRepository(context);
        var id = Guid.NewGuid();
        var movement = new Movement 
        { 
            MovementId = id,
            Cost = 100,
            Date = DateTime.Now,
            MovementType = MovementType.Bid,
            IsActive = true,
            Comments = "Test"
        };

        context.Movements.Add(movement);
        await context.SaveChangesAsync();

        var result = await repo.GetById(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.MovementId);
        Assert.Equal(100, result.Cost);
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenNotExists()
    {
        using var context = CreateContext();
        var repo = new MovementRepository(context);
        var nonExistentId = Guid.NewGuid();

        var result = await repo.GetById(nonExistentId);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_ReturnsAllMovements()
    {
        using var context = CreateContext();
        var repo = new MovementRepository(context);

        var movements = new[]
        {
            new Movement 
            { 
                MovementId = Guid.NewGuid(), 
                Cost = 50, 
                Date = DateTime.Now.AddDays(-1),
                MovementType = MovementType.Bid,
                IsActive = true,
                Comments = "Test"
            },
            new Movement 
            { 
                MovementId = Guid.NewGuid(), 
                Cost = 75, 
                Date = DateTime.Now,
                MovementType = MovementType.Contribution,
                IsActive = true,
                Comments = "Test"
            }
        };

        context.Movements.AddRange(movements);
        await context.SaveChangesAsync();

        var results = repo.GetAll().ToList();

        Assert.Equal(2, results.Count);
        Assert.Contains(results, m => m.Cost == 50);
        Assert.Contains(results, m => m.Cost == 75);
    }

    [Fact]
    public async Task CountAsync_ReturnsCorrectCount()
    {
        using var context = CreateContext();
        var repo = new MovementRepository(context);

        var movements = new[]
        {
            new Movement 
            { 
                MovementId = Guid.NewGuid(), 
                Cost = 50,
                Date = DateTime.Now,
                MovementType = MovementType.Bid,
                IsActive = true,
                Comments = "Test"
            },
            new Movement 
            { 
                MovementId = Guid.NewGuid(), 
                Cost = 75,
                Date = DateTime.Now,
                MovementType = MovementType.Contribution,
                IsActive = true,
                Comments = "Test"
            }
        };

        context.Movements.AddRange(movements);
        await context.SaveChangesAsync();

        var count = await repo.CountAsync();

        Assert.Equal(2, count);
    }

    [Fact]
    public async Task Register_AddsMovement()
    {
        using var context = CreateContext();
        var repo = new MovementRepository(context);
        var movement = new Movement 
        { 
            MovementId = Guid.NewGuid(), 
            Cost = 150,
            Date = DateTime.Now,
            MovementType = MovementType.Bid,
            IsActive = true,
            Comments = "Test"
        };

        var result = await repo.Register(movement);

        Assert.NotNull(result);
        Assert.Equal(150, result.Cost);
        Assert.True(result.IsActive);
        
        var countInDb = await context.Movements.CountAsync();
        Assert.Equal(1, countInDb);
        
        var movementInDb = await context.Movements.FindAsync(movement.MovementId);
        Assert.NotNull(movementInDb);
        Assert.Equal(150, movementInDb.Cost);
    }

    [Fact]
    public async Task Update_ModifiesExistingMovement()
    {
        using var context = CreateContext();
        var repo = new MovementRepository(context);
        var id = Guid.NewGuid();
        var movement = new Movement 
        { 
            MovementId = id, 
            Cost = 100,
            Date = DateTime.Now,
            MovementType = MovementType.Bid,
            IsActive = true,
            Comments = "Test"
        };

        context.Movements.Add(movement);
        await context.SaveChangesAsync();

        context.Entry(movement).State = EntityState.Detached;

        var updatedMovement = new Movement
        {
            MovementId = id,
            Cost = 200,
            Date = movement.Date,
            MovementType = movement.MovementType,
            IsActive = false,
            Comments = "Test"
        };

        var result = await repo.Update(updatedMovement);

        Assert.NotNull(result);
        Assert.Equal(200, result.Cost);
        Assert.False(result.IsActive);
        
        await context.SaveChangesAsync();
        
        var movementInDb = await context.Movements.FindAsync(id);
        Assert.NotNull(movementInDb);
        Assert.Equal(200, movementInDb.Cost);
        Assert.False(movementInDb.IsActive);
    }
}