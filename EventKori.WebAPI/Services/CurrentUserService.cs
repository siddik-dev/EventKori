using EventKori.Application.Interfaces;
using System.Security.Claims;

namespace EventKori.WebAPI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public int? DomainUserId 
    {
        get
        {
            var domainUserIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("domain_user_id")?.Value;
            if (int.TryParse(domainUserIdClaim, out int domainUserId))
            {
                return domainUserId;
            }
            return null;
        }
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
