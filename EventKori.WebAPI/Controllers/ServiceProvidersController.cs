using EventKori.Application.DTOs;
using EventKori.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;

namespace EventKori.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/service-providers")]
public class ServiceProvidersController : ControllerBase
{
    private readonly IServiceProviderService _serviceProviderService;

    public ServiceProvidersController(IServiceProviderService serviceProviderService)
    {
        _serviceProviderService = serviceProviderService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ServiceProviderDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? location,
        [FromQuery] bool verifiedOnly = false)
    {
        var providers = await _serviceProviderService.GetAllAsync(search, location, verifiedOnly);
        return Ok(providers);
    }

    [HttpGet("featured")]
    public async Task<ActionResult<IReadOnlyList<ServiceProviderDto>>> GetFeatured([FromQuery] int count = 6)
    {
        var providers = await _serviceProviderService.GetFeaturedAsync(count);
        return Ok(providers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceProviderDetailDto>> GetById(int id)
    {
        var provider = await _serviceProviderService.GetByIdAsync(id);
        return provider is null ? NotFound() : Ok(provider);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceProviderDto>> Create(CreateServiceProviderDto request)
    {
        try
        {
            var created = await _serviceProviderService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ServiceProviderDto>> Update(int id, UpdateServiceProviderDto request)
    {
        try
        {
            var updated = await _serviceProviderService.UpdateAsync(id, request);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _serviceProviderService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
