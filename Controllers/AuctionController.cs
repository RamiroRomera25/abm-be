using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Services.Interface;

namespace technical_tests_backend_ssr.Controllers;

[ApiController]
[Route("/api/v1/auctions")]
public class AuctionController : ControllerBase
{
    private readonly IAuctionService _auctionService;

    public AuctionController(IAuctionService auctionService)
    {
        _auctionService = auctionService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute]Guid id)
    {
        AuctionDto response = await _auctionService.GetById(id);
        return Ok(response);
    }
   
    [HttpGet("")]
    public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var response = await _auctionService.GetAll(pageNumber, pageSize);
        return Ok(response);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> Register([FromBody] AuctionDtoPost dtoPost)
    {
        var response = await _auctionService.Register(dtoPost);
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> SoftDelete([FromRoute] Guid id)
    {
        var response = await _auctionService.SoftDelete(id);
        return Ok(response);
    }
   
    [HttpPatch("reactivate/{id}")]
    public async Task<IActionResult> Reactivate([FromRoute] Guid id)
    {
        var response = await _auctionService.Reactivate(id);
        return Ok(response);
    }
   
    [HttpPut("")]
    public async Task<IActionResult> Update([FromBody] AuctionDtoPut dtoPost)
    {
        var response = await _auctionService.Update(dtoPost);
        return Ok(response);
    }
}
