using AutoMapper;
using Moq;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using technical_tests_backend_ssr.Services.Interface;
using Xunit;

namespace technical_tests_backend_ssr.UnitTests.Services
{
    public class AuctionServiceUnitTest
    {
        private readonly Mock<IAuctionRepository> _auctionRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AuctionService _service;

        public AuctionServiceUnitTest()
        {
            _auctionRepoMock = new Mock<IAuctionRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new AuctionService(_auctionRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetById_ReturnsAuctionDto()
        {
            var auctionId = Guid.NewGuid();
            var auctionEntity = new Auction { AuctionId = auctionId, Title = "Test Auction" };
            var auctionDto = new AuctionDto { AuctionId = auctionId, Title = "Test Auction" };

            _auctionRepoMock.Setup(r => r.GetById(auctionId)).ReturnsAsync(auctionEntity);
            _mapperMock.Setup(m => m.Map<AuctionDto>(auctionEntity)).Returns(auctionDto);

            var result = await _service.GetById(auctionId);

            Assert.NotNull(result);
            Assert.Equal(auctionId, result.AuctionId);
        }

        [Fact]
        public async Task GetById_WhenAuctionNotFound_ReturnsNull()
        {
            var auctionId = Guid.NewGuid();

            _auctionRepoMock.Setup(r => r.GetById(auctionId)).ReturnsAsync((Auction)null);
            _mapperMock.Setup(m => m.Map<AuctionDto>(null)).Returns((AuctionDto)null);

            var result = await _service.GetById(auctionId);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetModelById_ReturnsEntity()
        {
            var auctionId = Guid.NewGuid();
            var auctionEntity = new Auction { AuctionId = auctionId };

            _auctionRepoMock.Setup(r => r.GetById(auctionId)).ReturnsAsync(auctionEntity);

            var result = await _service.GetModelById(auctionId);

            Assert.Equal(auctionId, result.AuctionId);
        }

        [Fact]
        public async Task Register_SetsIsActiveAndIsOpen()
        {
            var request = new AuctionDtoPost { Title = "New Auction" };
            var entity = new Auction { Title = "New Auction" };
            var saved = new Auction { Title = "New Auction", IsActive = true, IsOpen = true };
            var dto = new AuctionDto { Title = "New Auction" };

            _mapperMock.Setup(m => m.Map<Auction>(request)).Returns(entity);
            _auctionRepoMock.Setup(r => r.Register(It.IsAny<Auction>())).ReturnsAsync(saved);
            _mapperMock.Setup(m => m.Map<AuctionDto>(saved)).Returns(dto);

            var result = await _service.Register(request);

            Assert.True(saved.IsActive);
            Assert.True(saved.IsOpen);
            Assert.Equal("New Auction", result.Title);
        }

        [Fact]
        public async Task Update_ReturnsUpdatedDto()
        {
            var request = new AuctionDtoPut { AuctionId = Guid.NewGuid(), Title = "Updated" };
            var entity = new Auction { AuctionId = request.AuctionId, Title = "Updated" };
            var updatedEntity = new Auction { AuctionId = request.AuctionId, Title = "Updated" };
            var dto = new AuctionDto { AuctionId = request.AuctionId, Title = "Updated" };

            _mapperMock.Setup(m => m.Map<Auction>(request)).Returns(entity);
            _auctionRepoMock.Setup(r => r.Update(entity)).ReturnsAsync(updatedEntity);
            _mapperMock.Setup(m => m.Map<AuctionDto>(updatedEntity)).Returns(dto);

            var result = await _service.Update(request);

            Assert.Equal("Updated", result.Title);
        }

        [Fact]
        public async Task SoftDelete_SetsIsActiveFalse()
        {
            var id = Guid.NewGuid();
            var auction = new Auction { AuctionId = id, IsActive = true };
            var updated = new Auction { AuctionId = id, IsActive = false };
            var dto = new AuctionDto { AuctionId = id };

            _auctionRepoMock.Setup(r => r.GetById(id)).ReturnsAsync(auction);
            _auctionRepoMock.Setup(r => r.Update(It.IsAny<Auction>())).ReturnsAsync(updated);
            _mapperMock.Setup(m => m.Map<AuctionDto>(It.IsAny<Auction>())).Returns(dto);

            var result = await _service.SoftDelete(id);

            Assert.False(updated.IsActive);
        }

        [Fact]
        public async Task Reactivate_SetsIsActiveTrue()
        {
            var id = Guid.NewGuid();
            var auction = new Auction { AuctionId = id, IsActive = false };
            var updated = new Auction { AuctionId = id, IsActive = true };
            var dto = new AuctionDto { AuctionId = id };

            _auctionRepoMock.Setup(r => r.GetById(id)).ReturnsAsync(auction);
            _auctionRepoMock.Setup(r => r.Update(It.IsAny<Auction>())).ReturnsAsync(updated);
            _mapperMock.Setup(m => m.Map<AuctionDto>(It.IsAny<Auction>())).Returns(dto);

            var result = await _service.Reactivate(id);

            Assert.True(updated.IsActive);
        }

        [Fact]
        public async Task UpdateHighestBid_UpdatesCurrentPrice()
        {
            var auction = new Auction { AuctionId = Guid.NewGuid(), CurrentPrice = 100 };
            var movement = new Movement { Auction = auction, Cost = 200 };

            _auctionRepoMock.Setup(r => r.Update(It.IsAny<Auction>())).ReturnsAsync(auction);

            await _service.UpdateHighestBid(movement);

            _auctionRepoMock.Verify(r => r.Update(It.Is<Auction>(a => a.CurrentPrice == 200)), Times.Once);
        }
    }
}
