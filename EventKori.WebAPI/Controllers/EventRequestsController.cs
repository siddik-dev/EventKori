using EventKori.Application.DTOs;
using EventKori.Application.Interfaces;
using EventKori.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;

namespace EventKori.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/event-requests")]
public class EventRequestsController : ControllerBase
{
    private readonly IEventRequestService _eventRequestService;
    private readonly ICurrentUserService _currentUserService;

    public EventRequestsController(IEventRequestService eventRequestService, ICurrentUserService currentUserService)
    {
        _eventRequestService = eventRequestService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EventRequestDto>>> GetAll(
        [FromQuery] string? eventType,
        [FromQuery] EventStatus? status)
    {
        // Customers only see their own events. ServiceProviders see all.
        int? userId = User.IsInRole("Customer") ? _currentUserService.DomainUserId : null;

        var events = await _eventRequestService.GetAllAsync(userId, eventType, status);
        return Ok(events);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventRequestDetailDto>> GetById(int id)
    {
        var eventRequest = await _eventRequestService.GetByIdAsync(id);
        return eventRequest is null ? NotFound() : Ok(eventRequest);
    }

    [HttpPost]
    public async Task<ActionResult<EventRequestDto>> Create(CreateEventRequestDto request)
    {
        try
        {
            if (User.IsInRole("Customer"))
            {
                request.UserId = _currentUserService.DomainUserId.GetValueOrDefault();
            }

            var created = await _eventRequestService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<EventRequestDto>> Update(int id, UpdateEventRequestDto request)
    {
        try
        {
            var updated = await _eventRequestService.UpdateAsync(id, request);
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
        var deleted = await _eventRequestService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
