using AutoMapper;
using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Dtos.Common;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;

namespace technical_tests_backend_ssr.Services.Interface;

public class AuctionService : IAuctionService
{
    private readonly IAuctionRepository _auctionRepository;
    
    private readonly IMapper _mapper;

    public AuctionService(IAuctionRepository auctionRepository,
                          IMapper mapper)
    {
        _auctionRepository = auctionRepository;
        _mapper = mapper;
    }
    
    public async Task<AuctionDto> GetById(Guid id)
    {
        AuctionDto dto = _mapper.Map<AuctionDto>(await this.GetModelById(id));
        return dto;
    }

    public async Task<Auction> GetModelById(Guid id)
    {
        return await _auctionRepository.GetById(id);
    }

    public async Task<PagedResult<AuctionDto>> GetAll(int pageNumber, int pageSize)
    {
        int skip = (pageNumber - 1) * pageSize;

        int totalCount = await _auctionRepository.CountAsync();

        List<Auction> entities = await _auctionRepository
            .GetAll()
            .Where(a => a.IsActive)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        List<AuctionDto> dtos = _mapper.Map<List<AuctionDto>>(entities);

        return new PagedResult<AuctionDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<AuctionDto> Register(AuctionDtoPost request)
    {
        Auction entity = _mapper.Map<Auction>(request);
        entity.IsActive = true;
        entity.IsOpen = true;
        return _mapper.Map<AuctionDto>(await _auctionRepository.Register(entity));
    }

    public async Task<AuctionDto> Update(AuctionDtoPut request)
    {
        Auction entity = _mapper.Map<Auction>(request);
        entity.IsActive = true;
        return _mapper.Map<AuctionDto>(await _auctionRepository.Update(entity));
    }

    public async Task<AuctionDto> SoftDelete(Guid id)
    {
        return await this.PerformIsActive(id, false);
    }

    public async Task<AuctionDto> Reactivate(Guid id)
    {
        return await this.PerformIsActive(id, true);
    }

    public async Task UpdateHighestBid(Movement movement)
    {
        Auction auction = movement.Auction;
        auction.CurrentPrice = movement.Cost;
        await _auctionRepository.Update(auction);
    }

    private async Task<AuctionDto> PerformIsActive(Guid id, bool isActive)
    {
        Auction entity = await _auctionRepository.GetById(id);
        entity.IsActive = isActive;
        return _mapper.Map<AuctionDto>(await _auctionRepository.Update(entity));
    }
}