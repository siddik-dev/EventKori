using EventKori.Application.DTOs;
using EventKori.Application.Interfaces;
using EventKori.Application.Mappers;
using EventKori.Domain.Entities;
using EventKori.Domain.Interfaces;

namespace EventKori.Application.Services;

public class ServiceProviderService : IServiceProviderService
{
    private readonly IUnitOfWork _unitOfWork;

    public ServiceProviderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ServiceProviderDto>> GetAllAsync(string? search = null, string? location = null, bool verifiedOnly = false)
    {
        var providers = verifiedOnly
            ? await _unitOfWork.ServiceProviders.GetVerifiedProvidersAsync()
            : await _unitOfWork.ServiceProviders.SearchAsync(search ?? string.Empty, location);

        if (verifiedOnly && (!string.IsNullOrWhiteSpace(search) || !string.IsNullOrWhiteSpace(location)))
        {
            providers = providers
                .Where(provider =>
                    (string.IsNullOrWhiteSpace(search) ||
                     provider.CompanyName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                     provider.ServicesOffered.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                     provider.Description.Contains(search, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(location) ||
                     provider.Location.Contains(location, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        return providers.Select(EventKoriMapper.ToDto).ToList();
    }

    public async Task<IReadOnlyList<ServiceProviderDto>> GetFeaturedAsync(int count = 6)
    {
        var providers = await _unitOfWork.ServiceProviders.GetFeaturedProvidersAsync(count);
        return providers.Select(EventKoriMapper.ToDto).ToList();
    }

    public async Task<ServiceProviderDetailDto?> GetByIdAsync(int id)
    {
        var provider = await _unitOfWork.ServiceProviders.GetWithDetailsAsync(id);
        return provider is null ? null : EventKoriMapper.ToDetailDto(provider);
    }

    public async Task<ServiceProviderDto> CreateAsync(CreateServiceProviderDto request)
    {
        ValidateCreateRequest(request);

        var provider = new ServiceProvider
        {
            UserId = request.UserId,
            CompanyName = request.CompanyName.Trim(),
            Description = request.Description.Trim(),
            ServicesOffered = request.ServicesOffered.Trim(),
            StartingPrice = request.StartingPrice,
            Location = request.Location.Trim(),
            YearsOfExperience = request.YearsOfExperience,
            IsVerified = request.IsVerified,
            BusinessLicense = request.BusinessLicense.Trim(),
            TaxId = request.TaxId.Trim(),
            Rating = 0,
            TotalReviews = 0
        };

        await _unitOfWork.ServiceProviders.AddAsync(provider);
        await _unitOfWork.CompleteAsync();

        var created = await _unitOfWork.ServiceProviders.GetWithDetailsAsync(provider.Id);
        return EventKoriMapper.ToDto(created ?? provider);
    }

    public async Task<ServiceProviderDto?> UpdateAsync(int id, UpdateServiceProviderDto request)
    {
        var provider = await _unitOfWork.ServiceProviders.GetByIdAsync(id);
        if (provider is null)
        {
            return null;
        }

        ValidateUpdateRequest(request);

        provider.CompanyName = request.CompanyName.Trim();
        provider.Description = request.Description.Trim();
        provider.ServicesOffered = request.ServicesOffered.Trim();
        provider.StartingPrice = request.StartingPrice;
        provider.Location = request.Location.Trim();
        provider.Rating = request.Rating;
        provider.TotalReviews = request.TotalReviews;
        provider.YearsOfExperience = request.YearsOfExperience;
        provider.IsVerified = request.IsVerified;
        provider.BusinessLicense = request.BusinessLicense.Trim();
        provider.TaxId = request.TaxId.Trim();

        await _unitOfWork.ServiceProviders.UpdateAsync(provider);
        await _unitOfWork.CompleteAsync();

        var updated = await _unitOfWork.ServiceProviders.GetWithDetailsAsync(id);
        return updated is null ? null : EventKoriMapper.ToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var provider = await _unitOfWork.ServiceProviders.GetByIdAsync(id);
        if (provider is null)
        {
            return false;
        }

        await _unitOfWork.ServiceProviders.DeleteAsync(provider);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    private static void ValidateCreateRequest(CreateServiceProviderDto request)
    {
        if (request.UserId <= 0)
        {
            throw new ArgumentException("An organizer user is required.");
        }

        ValidateCommon(request.CompanyName, request.ServicesOffered, request.Location, request.StartingPrice, request.YearsOfExperience);
    }

    private static void ValidateUpdateRequest(UpdateServiceProviderDto request)
    {
        ValidateCommon(request.CompanyName, request.ServicesOffered, request.Location, request.StartingPrice, request.YearsOfExperience);

        if (request.Rating < 0 || request.Rating > 5)
        {
            throw new ArgumentException("Rating must be between 0 and 5.");
        }
    }

    private static void ValidateCommon(string companyName, string servicesOffered, string location, decimal startingPrice, int yearsOfExperience)
    {
        if (string.IsNullOrWhiteSpace(companyName))
        {
            throw new ArgumentException("Company name is required.");
        }

        if (string.IsNullOrWhiteSpace(servicesOffered))
        {
            throw new ArgumentException("At least one offered service is required.");
        }

        if (string.IsNullOrWhiteSpace(location))
        {
            throw new ArgumentException("Location is required.");
        }

        if (startingPrice <= 0)
        {
            throw new ArgumentException("Starting price must be greater than zero.");
        }

        if (yearsOfExperience < 0)
        {
            throw new ArgumentException("Years of experience cannot be negative.");
        }
    }
}
