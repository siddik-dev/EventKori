using EventKori.Application.DTOs;
using EventKori.Application.Interfaces;
using EventKori.Domain.Entities;
using EventKori.Domain.Enums;
using EventKori.Domain.Interfaces;
using EventKori.Infrastructure.JWT;
using Microsoft.AspNetCore.Identity;

namespace EventKori.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto request)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new ArgumentException("Invalid email or password.");

        bool result = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!result)
            throw new ArgumentException("Invalid email or password.");

        IList<string> roles = await _userManager.GetRolesAsync(user);
        string token = _tokenService.GenerateToken(user, roles);

        User? domainUser = await _unitOfWork.Users.GetByIdAsync(user.DomainUserId.GetValueOrDefault());

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            FirstName = domainUser?.FirstName ?? "",
            LastName = domainUser?.LastName ?? "",
            Role = roles.FirstOrDefault() ?? "",
            UserType = domainUser?.Type ?? UserType.Customer
        };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto request)
    {
        ApplicationUser? existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            throw new ArgumentException("Email already exists.");

        // Determine role based on UserType
        string role = request.UserType == UserType.ServiceProvider ? "ServiceProvider" : "Customer";

        // Create Domain User
        var domainUser = new User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Type = request.UserType
        };

        await _unitOfWork.Users.AddAsync(domainUser);
        await _unitOfWork.CompleteAsync();

        var appUser = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            DomainUserId = domainUser.Id
        };

        IdentityResult result = await _userManager.CreateAsync(appUser, request.Password);
        if (!result.Succeeded)
        {
            // rollback domain user if identity fails
            await _unitOfWork.Users.DeleteAsync(domainUser);
            await _unitOfWork.CompleteAsync();

            string errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ArgumentException($"Registration failed: {errors}");
        }

        // Assign Role
        await _userManager.AddToRoleAsync(appUser, role);

        // Update domain user with identity id
        domainUser.IdentityId = appUser.Id;
        await _unitOfWork.Users.UpdateAsync(domainUser);
        await _unitOfWork.CompleteAsync();

        var roles = new List<string> { role };
        string token = _tokenService.GenerateToken(appUser, roles);

        return new AuthResponseDto
        {
            Token = token,
            Email = appUser.Email,
            FirstName = domainUser.FirstName,
            LastName = domainUser.LastName,
            Role = role,
            UserType = domainUser.Type
        };
    }
}
