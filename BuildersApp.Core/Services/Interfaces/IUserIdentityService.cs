using BuildersApp.Core.Models;

namespace BuildersApp.Core.Services.Interfaces;

public interface IUserIdentityService
{
    public User? CurrentUser { get; }
    public Task<bool> TrySetCurrentUser(string login);
    public Task<bool> TryGetCurrentUser();
    public Task Logout();
}