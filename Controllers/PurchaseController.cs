using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Services.Interface;

namespace technical_tests_backend_ssr.Controllers;

[ApiController]
[Route("/api/v1/purchases")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;

    public PurchaseController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute]Guid id)
    {
        var response = await _purchaseService.GetById(id);
        return Ok(response);
    }
   
    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] int page, [FromQuery] int size)
    {
        var response = await _purchaseService.GetAll(page, size);
        return Ok(response);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> Register([FromBody] PurchaseDtoPost dtoPost)
    {
        var response = await _purchaseService.Register(dtoPost);
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> SoftDelete([FromRoute] Guid id)
    {
        var response = await _purchaseService.SoftDelete(id);
        return Ok(response);
    }
   
    [HttpPatch("reactivate/{id}")]
    public async Task<IActionResult> Reactivate([FromRoute] Guid id)
    {
        var response = await _purchaseService.Reactivate(id);
        return Ok(response);
    }
   
    [HttpPut("")]
    public async Task<IActionResult> Update([FromBody] PurchaseDtoPut dtoPost)
    {
        var response = await _purchaseService.Update(dtoPost);
        return Ok(response);
    }
}