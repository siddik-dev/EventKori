using EventKori.Domain.Entities;
using EventKori.Domain.Enums;
using EventKori.Domain.Interfaces;
using EventKori.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EventKori.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(EventKoriDbContext context) : base(context)
        {
        }

        public async Task<User> GetByIdentityIdAsync(string identityId)
        {
            return await _entities.FirstOrDefaultAsync(u => u.IdentityId == identityId && u.IsActive);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _entities.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _entities.AnyAsync(u => u.Email == email && u.IsActive);
        }

        public async Task<IReadOnlyList<User>> GetServiceProvidersAsync()
        {
            return await _entities
                .Include(u => u.ServiceProvider)
                .Where(u => u.Type == UserType.ServiceProvider && u.IsActive)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<User>> GetUsersByTypeAsync(UserType userType)
        {
            return await _entities
                .Where(u => u.Type == userType && u.IsActive)
                .ToListAsync();
        }

        public async Task<User> GetUserWithServiceProviderAsync(int userId)
        {
            return await _entities
                .Include(u => u.ServiceProvider)
                .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
        }
    }
}
