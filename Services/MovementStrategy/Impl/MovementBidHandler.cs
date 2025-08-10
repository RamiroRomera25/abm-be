using AutoMapper;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Services.Interface;

namespace technical_tests_backend_ssr.Services.MovementStrategy.Impl;

public class MovementBidHandler :  IMovementStrategy
{
    private readonly IAuctionService _auctionService;
    
    private readonly IMapper _mapper;

    public MovementBidHandler(IMapper mapper, IAuctionService auctionService)
    {
        _auctionService = auctionService;
        _mapper = mapper;
    }
    
    public async Task<Movement> CreateMovement(MovementDtoPost dtoPost)
    {
        if (dtoPost.AuctionId == null)
        {
            throw new BadHttpRequestException("Auction ID null for BID movement");
        }
        Auction auction = await _auctionService.GetModelById(dtoPost.AuctionId.Value);
        
        this.Validate(auction, dtoPost);
        
        Movement movement = _mapper.Map<Movement>(dtoPost);
        movement.Auction = auction;
        await _auctionService.UpdateHighestBid(movement);
        return movement;
    }

    private void Validate(Auction auction, MovementDtoPost dtoPost)
    {
        if (auction.CurrentPrice >= dtoPost.Cost) {
            throw new BadHttpRequestException("The bid do not equal the highest bid.");
        }
        var now = DateTime.Now;
        if (now < auction.StartDate || now > auction.EndDate)
        {
            throw new BadHttpRequestException("The auction is outdated.");
        }
        if (!auction.IsOpen) {
            throw new BadHttpRequestException("The auction is closed.");
        }
        if (!auction.IsActive) {
            throw new BadHttpRequestException("The auction is deleted.");
        }
    }
}