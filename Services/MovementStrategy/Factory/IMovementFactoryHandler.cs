using technical_tests_backend_ssr.Models.Enums;

namespace technical_tests_backend_ssr.Services.MovementStrategy;

public interface IMovementFactoryHandler
{
    public IMovementStrategy GetHandler(MovementType movementType);
}