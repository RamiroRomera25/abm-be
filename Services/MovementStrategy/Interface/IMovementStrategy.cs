using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Services.MovementStrategy;

public interface IMovementStrategy
{
    Task<Movement> CreateMovement(MovementDtoPost dtoPost);
}