namespace EventKori.Application.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    int? DomainUserId { get; }
    bool IsAuthenticated { get; }
}
