using AutoMapper;
using Moq;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Models.Enums;
using technical_tests_backend_ssr.Services.Interface;
using technical_tests_backend_ssr.Services.MovementStrategy.Impl;
using Xunit;

namespace technical_tests_backend_ssr.UnitTests.Services;

public class MovementContributionHandlerUnitTest
{
    private readonly Mock<IPurchaseService> _mockPurchaseService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly MovementContributionHandler _handler;

    public MovementContributionHandlerUnitTest()
    {
     _mockPurchaseService = new Mock<IPurchaseService>();
     _mockMapper = new Mock<IMapper>();
     _handler = new MovementContributionHandler(_mockMapper.Object, _mockPurchaseService.Object);
    }

    [Fact]
    public async Task CreateMovement_Should_Create_Movement_When_Valid_Contribution()
    {
     var purchaseId = Guid.NewGuid();
     var request = new MovementDtoPost
     {
         PurchaseId = purchaseId,
         Cost = 50,
         MovementType = MovementType.Contribution
     };
     var purchase = new Purchase
     {
         PurchaseId = purchaseId,
         Title = "Test Purchase",
         MoneyCollected = 100,
         TargetPrice = 200,
         StartDate = DateTime.Now.AddDays(-1),
         EndDate = DateTime.Now.AddDays(1),
         IsActive = true
     };
     var movement = new Movement { Cost = 50, MovementType = MovementType.Contribution };

     _mockPurchaseService.Setup(x => x.GetModelById(purchaseId)).ReturnsAsync(purchase);
     _mockMapper.Setup(x => x.Map<Movement>(request)).Returns(movement);
     _mockPurchaseService.Setup(x => x.UpdateCurrentAmount(movement)).Returns(Task.CompletedTask);

     var result = await _handler.CreateMovement(request);

     Assert.NotNull(result);
     Assert.Equal(purchase, result.Purchase);
     _mockPurchaseService.Verify(x => x.GetModelById(purchaseId), Times.Once);
     _mockPurchaseService.Verify(x => x.UpdateCurrentAmount(movement), Times.Once);
     _mockMapper.Verify(x => x.Map<Movement>(request), Times.Once);
    }

    [Fact]
    public async Task CreateMovement_Should_Throw_BadHttpRequestException_When_PurchaseId_Is_Null()
    {
     var request = new MovementDtoPost
     {
         PurchaseId = null,
         Cost = 50
     };

     var exception = await Assert.ThrowsAsync<BadHttpRequestException>(() => _handler.CreateMovement(request));
     Assert.Equal("Purchase ID null for CONTRIBUTION movement", exception.Message);
    }

    [Fact]
    public async Task CreateMovement_Should_Throw_BadHttpRequestException_When_Purchase_Already_Completed()
    {
     var purchaseId = Guid.NewGuid();
     var request = new MovementDtoPost
     {
         PurchaseId = purchaseId,
         Cost = 50
     };
     var purchase = new Purchase
     {
         PurchaseId = purchaseId,
         Title = "Test Purchase",
         MoneyCollected = 200,
         TargetPrice = 200,
         StartDate = DateTime.Now.AddDays(-1),
         EndDate = DateTime.Now.AddDays(1),
         IsActive = true
     };

     _mockPurchaseService.Setup(x => x.GetModelById(purchaseId)).ReturnsAsync(purchase);

     var exception = await Assert.ThrowsAsync<BadHttpRequestException>(() => _handler.CreateMovement(request));
     Assert.Equal("The purchase has already completed.", exception.Message);
    }

    [Fact]
    public async Task CreateMovement_Should_Throw_BadHttpRequestException_When_Purchase_Is_Outdated()
    {
     var purchaseId = Guid.NewGuid();
     var request = new MovementDtoPost
     {
         PurchaseId = purchaseId,
         Cost = 50
     };
     var purchase = new Purchase
     {
         PurchaseId = purchaseId,
         Title = "Test Purchase",
         MoneyCollected = 100,
         TargetPrice = 200,
         StartDate = DateTime.Now.AddDays(-2),
         EndDate = DateTime.Now.AddDays(-1),
         IsActive = true
     };

     _mockPurchaseService.Setup(x => x.GetModelById(purchaseId)).ReturnsAsync(purchase);

     var exception = await Assert.ThrowsAsync<BadHttpRequestException>(() => _handler.CreateMovement(request));
     Assert.Equal("The purchase is outdated.", exception.Message);
    }

    [Fact]
    public async Task CreateMovement_Should_Throw_BadHttpRequestException_When_Purchase_Is_Deleted()
    {
     var purchaseId = Guid.NewGuid();
     var request = new MovementDtoPost
     {
         PurchaseId = purchaseId,
         Cost = 50
     };
     var purchase = new Purchase
     {
         PurchaseId = purchaseId,
         Title = "Test Purchase",
         MoneyCollected = 100,
         TargetPrice = 200,
         StartDate = DateTime.Now.AddDays(-1),
         EndDate = DateTime.Now.AddDays(1),
         IsActive = false
     };

     _mockPurchaseService.Setup(x => x.GetModelById(purchaseId)).ReturnsAsync(purchase);

     var exception = await Assert.ThrowsAsync<BadHttpRequestException>(() => _handler.CreateMovement(request));
     Assert.Equal("The purchase is deleted.", exception.Message);
    }
    }
