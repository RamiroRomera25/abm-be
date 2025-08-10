using AutoMapper;
using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Dtos.Common;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using technical_tests_backend_ssr.Services.Interface;

namespace technical_tests_backend_ssr.Services.Impl;

public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository _purchaseRepository;
    
    private readonly IMapper _mapper;
    
    public PurchaseService(IPurchaseRepository purchaseRepository, 
        IMapper mapper)
    {
        _purchaseRepository = purchaseRepository;
        _mapper = mapper;
    }
    
    public async Task<PurchaseDto> GetById(Guid id)
    {
        PurchaseDto dto = _mapper.Map<PurchaseDto>(await this.GetModelById(id));
        return dto;
    }

    public async Task<Purchase> GetModelById(Guid id)
    {
        return await _purchaseRepository.GetById(id);
    }

    public async Task<PagedResult<PurchaseDto>> GetAll(int pageNumber, int pageSize)
    {
        int skip = (pageNumber - 1) * pageSize;

        int totalCount = await _purchaseRepository.CountAsync();

        List<Purchase> entities = await _purchaseRepository
            .GetAll()
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        List<PurchaseDto> dtos = _mapper.Map<List<PurchaseDto>>(entities);

        return new PagedResult<PurchaseDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<PurchaseDto> Register(PurchaseDtoPost request)
    {
        Purchase entity = _mapper.Map<Purchase>(request);
        entity.IsActive = true;
        return _mapper.Map<PurchaseDto>(await _purchaseRepository.Register(entity));
    }

    public async Task<PurchaseDto> Update(PurchaseDtoPut request)
    {
        Purchase entity = _mapper.Map<Purchase>(request);
        return _mapper.Map<PurchaseDto>(await _purchaseRepository.Update(entity));
    }

    public async Task<PurchaseDto> SoftDelete(Guid id)
    {
        return await this.PerformIsActive(id, false);
    }

    public async Task<PurchaseDto> Reactivate(Guid id)
    {
        return await this.PerformIsActive(id, true);
    }

    public async Task UpdateCurrentAmount(Movement movement)
    {
        Purchase purchase = movement.Purchase;

        purchase.MoneyCollected += movement.Cost;

        if (purchase.MoneyCollected >= purchase.TargetPrice)
        {
            purchase.Stock--;
        }
        
        await _purchaseRepository.Update(purchase);
    }

    private async Task<PurchaseDto> PerformIsActive(Guid id, bool isActive)
    {
        Purchase entity = await _purchaseRepository.GetById(id);
        entity.IsActive = false;
        return _mapper.Map<PurchaseDto>(_purchaseRepository.Update(entity));
    }
}