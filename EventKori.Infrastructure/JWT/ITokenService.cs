using EventKori.Infrastructure.Identity;

namespace EventKori.Infrastructure.JWT
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
    }
}
