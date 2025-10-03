using EventKori.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
