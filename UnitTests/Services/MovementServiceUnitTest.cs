using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Dtos.Common;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Models.Enums;
using technical_tests_backend_ssr.Repositories;
using technical_tests_backend_ssr.Services.Impl;
using technical_tests_backend_ssr.Services.Interface;
using technical_tests_backend_ssr.Services.MovementStrategy;

namespace technical_tests_backend_ssr.Tests.Services
{
    public class MovementServiceUnitTest
    {
        private readonly Mock<IMovementRepository> _mockMovementRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMovementFactoryHandler> _mockMovementFactoryHandler;
        private readonly MovementService _movementService;

        public MovementServiceUnitTest()
        {
            _mockMovementRepository = new Mock<IMovementRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockMovementFactoryHandler = new Mock<IMovementFactoryHandler>();
            _movementService = new MovementService(_mockMovementRepository.Object, _mockMapper.Object, _mockMovementFactoryHandler.Object);
        }

        [Fact]
        public async Task GetById_Should_Return_MovementDto_When_Valid_Id()
        {
            var id = Guid.NewGuid();
            var movement = new Movement { MovementId = id, Cost = 100, Date = DateTime.Now };
            var movementDto = new MovementDto { MovementId = id, Cost = 100, Date = movement.Date };

            _mockMovementRepository.Setup(x => x.GetById(id)).ReturnsAsync(movement);
            _mockMapper.Setup(x => x.Map<MovementDto>(movement)).Returns(movementDto);

            var result = await _movementService.GetById(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.MovementId);
            Assert.Equal(100, result.Cost);
            _mockMovementRepository.Verify(x => x.GetById(id), Times.Once);
            _mockMapper.Verify(x => x.Map<MovementDto>(movement), Times.Once);
        }

        [Fact]
        public async Task Register_Should_Create_Movement_When_Valid_Request()
        {
            var request = new MovementDtoPost
            {
                MovementType = MovementType.Bid,
                Cost = 100,
                AuctionId = Guid.NewGuid()
            };
            var movement = new Movement { MovementId = Guid.NewGuid(), Cost = 100 };
            var registeredMovement = new Movement { MovementId = movement.MovementId, Cost = 100, IsActive = true };
            var movementDto = new MovementDto { MovementId = movement.MovementId, Cost = 100, IsActive = true };

            var mockStrategy = new Mock<IMovementStrategy>();
            mockStrategy.Setup(x => x.CreateMovement(request)).ReturnsAsync(movement);
            _mockMovementFactoryHandler.Setup(x => x.GetHandler(request.MovementType)).Returns(mockStrategy.Object);
            _mockMovementRepository.Setup(x => x.Register(It.IsAny<Movement>())).ReturnsAsync(registeredMovement);
            _mockMapper.Setup(x => x.Map<MovementDto>(registeredMovement)).Returns(movementDto);

            var result = await _movementService.Register(request);

            Assert.NotNull(result);
            Assert.True(result.IsActive);
            Assert.True(movement.IsActive);
            _mockMovementFactoryHandler.Verify(x => x.GetHandler(request.MovementType), Times.Once);
            mockStrategy.Verify(x => x.CreateMovement(request), Times.Once);
            _mockMovementRepository.Verify(x => x.Register(It.Is<Movement>(m => m.IsActive)), Times.Once);
        }

        [Fact]
        public async Task SoftDelete_Should_Set_IsActive_To_False()
        {
            var id = Guid.NewGuid();
            var movement = new Movement { MovementId = id, IsActive = true };
            var updatedMovement = new Movement { MovementId = id, IsActive = false };
            var movementDto = new MovementDto { MovementId = id, IsActive = false };

            _mockMovementRepository.Setup(x => x.GetById(id)).ReturnsAsync(movement);
            _mockMovementRepository.Setup(x => x.Update(It.IsAny<Movement>())).Returns(Task.FromResult(updatedMovement));
            _mockMapper.Setup(x => x.Map<MovementDto>(updatedMovement)).Returns(movementDto);

            var result = await _movementService.SoftDelete(id);

            _mockMovementRepository.Verify(x => x.GetById(id), Times.Once);
            _mockMovementRepository.Verify(x => x.Update(movement), Times.Once);
        }

        [Fact]
        public async Task Reactivate_Should_Set_IsActive_To_True()
        {
            var id = Guid.NewGuid();
            var movement = new Movement { MovementId = id, IsActive = false };
            var updatedMovement = new Movement { MovementId = id, IsActive = false };
            var movementDto = new MovementDto { MovementId = id, IsActive = false };

            _mockMovementRepository.Setup(x => x.GetById(id)).ReturnsAsync(movement);
            _mockMovementRepository.Setup(x => x.Update(It.IsAny<Movement>())).Returns(Task.FromResult(updatedMovement));
            _mockMapper.Setup(x => x.Map<MovementDto>(updatedMovement)).Returns(movementDto);

            var result = await _movementService.Reactivate(id);

            _mockMovementRepository.Verify(x => x.GetById(id), Times.Once);
            _mockMovementRepository.Verify(x => x.Update(movement), Times.Once);
        }
    }
}