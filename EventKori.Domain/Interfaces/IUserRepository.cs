using EventKori.Domain.Entities;

namespace EventKori.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByIdentityIdAsync(string identityId);
        Task<User> GetByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);
        Task<IReadOnlyList<User>> GetServiceProvidersAsync();
    }
}
