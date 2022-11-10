using BuildersApp.Core.Models;

namespace BuildersApp.Core.Services.Interfaces;

public interface IAuthorizationService
{
    public Task<bool> Register(User user);
    public Task<bool> Authorize(LoginCredentials loginCredentials);
    public Task Logout();
}