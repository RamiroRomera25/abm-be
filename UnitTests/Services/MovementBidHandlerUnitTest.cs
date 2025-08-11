using AutoMapper;
using Moq;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Models.Enums;
using technical_tests_backend_ssr.Services.Interface;
using technical_tests_backend_ssr.Services.MovementStrategy.Impl;
using Xunit;

namespace technical_tests_backend_ssr.UnitTests.Services;

public class MovementBidHandlerUnitTest
{
    private readonly Mock<IAuctionService> _mockAuctionService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly MovementBidHandler _handler;

    public MovementBidHandlerUnitTest()
    {
        _mockAuctionService = new Mock<IAuctionService>();
        _mockMapper = new Mock<IMapper>();
        _handler = new MovementBidHandler(_mockMapper.Object, _mockAuctionService.Object);
    }

    [Fact]
    public async Task CreateMovement_Should_Create_Movement_When_Valid_Bid()
    {
        var auctionId = Guid.NewGuid();
        var request = new MovementDtoPost
        {
            AuctionId = auctionId,
            Cost = 150,
            MovementType = MovementType.Bid
        };
        var auction = new Auction
        {
            AuctionId = auctionId,
            Title = "Test Auction",
            CurrentPrice = 100,
            StartDate = DateTime.Now.AddDays(-1),
            EndDate = DateTime.Now.AddDays(1),
            IsOpen = true,
            IsActive = true
        };
        var movement = new Movement { Cost = 150, MovementType = MovementType.Bid };

        _mockAuctionService.Setup(x => x.GetModelById(auctionId)).ReturnsAsync(auction);
        _mockMapper.Setup(x => x.Map<Movement>(request)).Returns(movement);
        _mockAuctionService.Setup(x => x.UpdateHighestBid(movement)).Returns(Task.CompletedTask);

        var result = await _handler.CreateMovement(request);

        Assert.NotNull(result);
        Assert.Equal(auction, result.Auction);
        _mockAuctionService.Verify(x => x.GetModelById(auctionId), Times.Once);
        _mockAuctionService.Verify(x => x.UpdateHighestBid(movement), Times.Once);
        _mockMapper.Verify(x => x.Map<Movement>(request), Times.Once);
    }

    [Fact]
    public async Task CreateMovement_Should_Throw_BadHttpRequestException_When_AuctionId_Is_Null()
    {
        var request = new MovementDtoPost
        {
            AuctionId = null,
            Cost = 150
        };

        var exception = await Assert.ThrowsAsync<BadHttpRequestException>(() => _handler.CreateMovement(request));
        Assert.Equal("Auction ID null for BID movement", exception.Message);
    }

    [Fact]
    public async Task CreateMovement_Should_Throw_BadHttpRequestException_When_Bid_Is_Not_Higher_Than_Current()
    {
        var auctionId = Guid.NewGuid();
        var request = new MovementDtoPost
        {
            AuctionId = auctionId,
            Cost = 100
        };
        var auction = new Auction
        {
            AuctionId = auctionId,
            Title = "Test Auction",
            CurrentPrice = 100,
            StartDate = DateTime.Now.AddDays(-1),
            EndDate = DateTime.Now.AddDays(1),
            IsOpen = true,
            IsActive = true
        };

        _mockAuctionService.Setup(x => x.GetModelById(auctionId)).ReturnsAsync(auction);

        var exception = await Assert.ThrowsAsync<BadHttpRequestException>(() => _handler.CreateMovement(request));
        Assert.Equal("The bid do not equal the highest bid.", exception.Message);
    }

    [Fact]
    public async Task CreateMovement_Should_Throw_BadHttpRequestException_When_Auction_Is_Outdated()
    {
        var auctionId = Guid.NewGuid();
        var request = new MovementDtoPost
        {
            AuctionId = auctionId,
            Cost = 150
        };
        var auction = new Auction
        {
            AuctionId = auctionId,
            Title = "Test Auction",
            CurrentPrice = 100,
            StartDate = DateTime.Now.AddDays(-2),
            EndDate = DateTime.Now.AddDays(-1),
            IsOpen = true,
            IsActive = true
        };

        _mockAuctionService.Setup(x => x.GetModelById(auctionId)).ReturnsAsync(auction);

        var exception = await Assert.ThrowsAsync<BadHttpRequestException>(() => _handler.CreateMovement(request));
        Assert.Equal("The auction is outdated.", exception.Message);
    }

    [Fact]
    public async Task CreateMovement_Should_Throw_BadHttpRequestException_When_Auction_Is_Closed()
    {
        var auctionId = Guid.NewGuid();
        var request = new MovementDtoPost
        {
            AuctionId = auctionId,
            Cost = 150
        };
        var auction = new Auction
        {
            AuctionId = auctionId,
            Title = "Test Auction",
            CurrentPrice = 100,
            StartDate = DateTime.Now.AddDays(-1),
            EndDate = DateTime.Now.AddDays(1),
            IsOpen = false,
            IsActive = true
        };

        _mockAuctionService.Setup(x => x.GetModelById(auctionId)).ReturnsAsync(auction);

        var exception = await Assert.ThrowsAsync<BadHttpRequestException>(() => _handler.CreateMovement(request));
        Assert.Equal("The auction is closed.", exception.Message);
    }

    [Fact]
    public async Task CreateMovement_Should_Throw_BadHttpRequestException_When_Auction_Is_Deleted()
    {
        var auctionId = Guid.NewGuid();
        var request = new MovementDtoPost
        {
            AuctionId = auctionId,
            Cost = 150
        };
        var auction = new Auction
        {
            AuctionId = auctionId,
            Title = "Test Auction",
            CurrentPrice = 100,
            StartDate = DateTime.Now.AddDays(-1),
            EndDate = DateTime.Now.AddDays(1),
            IsOpen = true,
            IsActive = false
        };

        _mockAuctionService.Setup(x => x.GetModelById(auctionId)).ReturnsAsync(auction);

        var exception = await Assert.ThrowsAsync<BadHttpRequestException>(() => _handler.CreateMovement(request));
        Assert.Equal("The auction is deleted.", exception.Message);
    }
}