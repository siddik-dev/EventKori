using EventKori.Application.DTOs;

namespace EventKori.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto request);
    Task<AuthResponseDto> RegisterAsync(RegisterDto request);
}
