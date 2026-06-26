using EventKori.Application.DTOs;

namespace EventKori.Application.Interfaces;

public interface IServiceProviderService
{
    Task<IReadOnlyList<ServiceProviderDto>> GetAllAsync(string? search = null, string? location = null, bool verifiedOnly = false);
    Task<IReadOnlyList<ServiceProviderDto>> GetFeaturedAsync(int count = 6);
    Task<ServiceProviderDetailDto?> GetByIdAsync(int id);
    Task<ServiceProviderDto> CreateAsync(CreateServiceProviderDto request);
    Task<ServiceProviderDto?> UpdateAsync(int id, UpdateServiceProviderDto request);
    Task<bool> DeleteAsync(int id);
}
