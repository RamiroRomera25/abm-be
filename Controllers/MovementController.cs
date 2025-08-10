using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Services.Interface;

namespace technical_tests_backend_ssr.Controllers;

[ApiController]
[Route("/api/v1/movements")]
public class MovementController : ControllerBase
{
    private readonly IMovementService _movementService;

    public MovementController(IMovementService movementService)
    {
        _movementService = movementService;
    }
   
    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] int page, [FromQuery] int size)
    {
        var response = await _movementService.GetAll(page, size);
        return Ok(response);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> Register([FromBody] MovementDtoPost dtoPost)
    {
        var response = await _movementService.Register(dtoPost);
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> SoftDelete([FromRoute] Guid id)
    {
        var response = await _movementService.SoftDelete(id);
        return Ok(response);
    }
   
    [HttpPatch("reactivate/{id}")]
    public async Task<IActionResult> Reactivate([FromRoute] Guid id)
    {
        var response = await _movementService.Reactivate(id);
        return Ok(response);
    }
}