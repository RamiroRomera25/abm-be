using AutoMapper;
using Moq;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using technical_tests_backend_ssr.Services.Impl;
using Xunit;

namespace technical_tests_backend_ssr.UnitTests.Services
{
    public class PurchaseServiceTests
    {
        private readonly Mock<IPurchaseRepository> _purchaseRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PurchaseService _service;

        public PurchaseServiceTests()
        {
            _purchaseRepoMock = new Mock<IPurchaseRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new PurchaseService(_purchaseRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetById_ReturnsPurchaseDto()
        {
            var id = Guid.NewGuid();
            var entity = new Purchase { PurchaseId = id, Title = "Test Purchase" };
            var dto = new PurchaseDto { PurchaseId = id, Title = "Test Purchase" };

            _purchaseRepoMock.Setup(r => r.GetById(id)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<PurchaseDto>(entity)).Returns(dto);

            var result = await _service.GetById(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.PurchaseId);
        }

        [Fact]
        public async Task GetById_WhenNotFound_ReturnsNull()
        {
            var id = Guid.NewGuid();

            _purchaseRepoMock.Setup(r => r.GetById(id)).ReturnsAsync((Purchase)null);
            _mapperMock.Setup(m => m.Map<PurchaseDto>(null)).Returns((PurchaseDto)null);

            var result = await _service.GetById(id);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetModelById_ReturnsEntity()
        {
            var id = Guid.NewGuid();
            var entity = new Purchase { PurchaseId = id };

            _purchaseRepoMock.Setup(r => r.GetById(id)).ReturnsAsync(entity);

            var result = await _service.GetModelById(id);

            Assert.Equal(id, result.PurchaseId);
        }

        [Fact]
        public async Task Register_SetsIsActiveTrue()
        {
            var request = new PurchaseDtoPost { Title = "New Purchase" };
            var entity = new Purchase { Title = "New Purchase" };
            var saved = new Purchase { Title = "New Purchase", IsActive = true };
            var dto = new PurchaseDto { Title = "New Purchase" };

            _mapperMock.Setup(m => m.Map<Purchase>(request)).Returns(entity);
            _purchaseRepoMock.Setup(r => r.Register(It.IsAny<Purchase>())).ReturnsAsync(saved);
            _mapperMock.Setup(m => m.Map<PurchaseDto>(saved)).Returns(dto);

            var result = await _service.Register(request);

            Assert.True(saved.IsActive);
            Assert.Equal("New Purchase", result.Title);
        }

        [Fact]
        public async Task Update_ReturnsUpdatedDto()
        {
            var request = new PurchaseDtoPut { PurchaseId = Guid.NewGuid(), Title = "Updated" };
            var entity = new Purchase { PurchaseId = request.PurchaseId, Title = "Updated" };
            var updated = new Purchase { PurchaseId = request.PurchaseId, Title = "Updated" };
            var dto = new PurchaseDto { PurchaseId = request.PurchaseId, Title = "Updated" };

            _mapperMock.Setup(m => m.Map<Purchase>(request)).Returns(entity);
            _purchaseRepoMock.Setup(r => r.Update(entity)).ReturnsAsync(updated);
            _mapperMock.Setup(m => m.Map<PurchaseDto>(updated)).Returns(dto);

            var result = await _service.Update(request);

            Assert.Equal("Updated", result.Title);
        }

        [Fact]
        public async Task SoftDelete_SetsIsActiveFalse()
        {
            var id = Guid.NewGuid();
            var entity = new Purchase { PurchaseId = id, IsActive = true };
            var updated = new Purchase { PurchaseId = id, IsActive = false };
            var dto = new PurchaseDto { PurchaseId = id };

            _purchaseRepoMock.Setup(r => r.GetById(id)).ReturnsAsync(entity);
            _purchaseRepoMock.Setup(r => r.Update(It.IsAny<Purchase>())).ReturnsAsync(updated);
            _mapperMock.Setup(m => m.Map<PurchaseDto>(It.IsAny<Purchase>())).Returns(dto);

            var result = await _service.SoftDelete(id);

            Assert.False(updated.IsActive);
        }

        [Fact]
        public async Task Reactivate_SetsIsActiveTrue()
        {
            var id = Guid.NewGuid();
            var entity = new Purchase { PurchaseId = id, IsActive = false };
            var updated = new Purchase { PurchaseId = id, IsActive = true };
            var dto = new PurchaseDto { PurchaseId = id };

            _purchaseRepoMock.Setup(r => r.GetById(id)).ReturnsAsync(entity);
            _purchaseRepoMock.Setup(r => r.Update(It.IsAny<Purchase>())).ReturnsAsync(updated);
            _mapperMock.Setup(m => m.Map<PurchaseDto>(It.IsAny<Purchase>())).Returns(dto);

            var result = await _service.Reactivate(id);

            Assert.True(updated.IsActive);
        }

        [Fact]
        public async Task UpdateCurrentAmount_AddsMoney_AndDoesNotReduceStock_WhenBelowTarget()
        {
            var purchase = new Purchase { MoneyCollected = 50, TargetPrice = 200, Stock = 10 };
            var movement = new Movement { Purchase = purchase, Cost = 30 };

            _purchaseRepoMock.Setup(r => r.Update(It.IsAny<Purchase>())).ReturnsAsync(purchase);

            await _service.UpdateCurrentAmount(movement);

            Assert.Equal(80, purchase.MoneyCollected);
            Assert.Equal(10, purchase.Stock);
        }

        [Fact]
        public async Task UpdateCurrentAmount_ReducesStock_WhenTargetReached()
        {
            var purchase = new Purchase { MoneyCollected = 150, TargetPrice = 150, Stock = 5 };
            var movement = new Movement { Purchase = purchase, Cost = 20 };

            _purchaseRepoMock.Setup(r => r.Update(It.IsAny<Purchase>())).ReturnsAsync(purchase);

            await _service.UpdateCurrentAmount(movement);

            Assert.Equal(170, purchase.MoneyCollected);
            Assert.Equal(4, purchase.Stock);
        }
    }
}
