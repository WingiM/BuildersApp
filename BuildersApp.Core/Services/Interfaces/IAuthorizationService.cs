using BuildersApp.Core.Models;

namespace BuildersApp.Core.Services.Interfaces;

public interface IAuthorizationService
{
    public Task<bool> RegisterAsync(User user);
    public Task<bool> AuthorizeAsync(LoginCredentials loginCredentials);
    public Task LogoutAsync();
}