using AutoMapper;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Services.Interface;

namespace technical_tests_backend_ssr.Services.MovementStrategy.Impl;

public class MovementContributionHandler : IMovementStrategy
{
    private readonly IPurchaseService _purchaseService;
    
    private readonly IMapper _mapper;

    public MovementContributionHandler(IMapper mapper, IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
        _mapper = mapper;
    }
    
    public async Task<Movement> CreateMovement(MovementDtoPost dtoPost)
    {
        if (dtoPost.PurchaseId == null)
        {
            throw new BadHttpRequestException("Purchase ID null for CONTRIBUTION movement");
        }
        Purchase purchase = await _purchaseService.GetModelById(dtoPost.PurchaseId.Value);
        
        this.Validate(purchase, dtoPost);
        
        Movement movement = _mapper.Map<Movement>(dtoPost);
        movement.Purchase = purchase;
        await _purchaseService.UpdateCurrentAmount(movement);
        return movement;
    }
    
    private void Validate(Purchase purchase, MovementDtoPost dtoPost)
    {
        if (purchase.MoneyCollected >= purchase.TargetPrice) {
            throw new BadHttpRequestException("The purchase has already completed.");
        }
        var now = DateTime.Now;
        if (now < purchase.StartDate || now > purchase.EndDate)
        {
            throw new BadHttpRequestException("The purchase is outdated.");
        }
        if (!purchase.IsActive) {
            throw new BadHttpRequestException("The purchase is deleted.");
        }
    }
}