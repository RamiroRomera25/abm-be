using technical_tests_backend_ssr.Models.Enums;
using technical_tests_backend_ssr.Services.MovementStrategy.Impl;

namespace technical_tests_backend_ssr.Services.MovementStrategy;

public class MovementFactoryHandler : IMovementFactoryHandler
{
    private readonly Dictionary<MovementType, IMovementStrategy> _handlers;

    public MovementFactoryHandler(
        MovementBidHandler movementBidHandler,
        MovementContributionHandler movementContributionHandler
    )
    {
        _handlers = new Dictionary<MovementType, IMovementStrategy>
        {
            { MovementType.Bid, movementBidHandler },
            { MovementType.Contribution, movementContributionHandler }
        };
    }

    public IMovementStrategy GetHandler(MovementType movementType)
    {
        return _handlers.TryGetValue(movementType, out var handler)
            ? handler
            : throw new KeyNotFoundException($"No handler found for movement type {movementType}");
    }
}