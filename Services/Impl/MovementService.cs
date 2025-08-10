using AutoMapper;
using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Dtos.Common;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using technical_tests_backend_ssr.Services.Interface;
using technical_tests_backend_ssr.Services.MovementStrategy;

namespace technical_tests_backend_ssr.Services.Impl;

public class MovementService : IMovementService
{
    private readonly IMovementRepository _movementRepository;
    
    private readonly IMapper _mapper;
    
    private readonly IMovementFactoryHandler _movementFactoryHandler;

    public MovementService(IMovementRepository movementRepository, 
                           IMapper mapper,
                           IMovementFactoryHandler movementFactoryHandler)
    {
        _movementRepository = movementRepository;
        _mapper = mapper;
        _movementFactoryHandler = movementFactoryHandler;
    }
    
    public async Task<MovementDto> GetById(Guid id)
    {
        Movement entity = await _movementRepository.GetById(id);
        MovementDto dto = _mapper.Map<MovementDto>(entity);
        return dto;
    }

    public async Task<PagedResult<MovementDto>> GetAll(int pageNumber, int pageSize)
    {
        int skip = (pageNumber - 1) * pageSize;

        int totalCount = await _movementRepository.CountAsync();

        List<Movement> entities = await _movementRepository
            .GetAll()
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        List<MovementDto> dtos = _mapper.Map<List<MovementDto>>(entities);

        return new PagedResult<MovementDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<MovementDto> Register(MovementDtoPost request)
    {
        Movement entity = await _movementFactoryHandler.GetHandler(request.MovementType).CreateMovement(request);
        entity.IsActive = true;
        return _mapper.Map<MovementDto>(await _movementRepository.Register(entity));
    }

    public async Task<MovementDto> SoftDelete(Guid id)
    {
        return await this.PerformIsActive(id, false);
    }

    public async Task<MovementDto> Reactivate(Guid id)
    {
        return await this.PerformIsActive(id, true);
    }
    
    private async Task<MovementDto> PerformIsActive(Guid id, bool isActive)
    {
        Movement entity = await _movementRepository.GetById(id);
        entity.IsActive = false;
        return _mapper.Map<MovementDto>(_movementRepository.Update(entity));
    }
}