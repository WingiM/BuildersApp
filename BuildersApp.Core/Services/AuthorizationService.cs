using System.Text;
using BuildersApp.Core.Models;
using BuildersApp.Core.Models.UserInfo;
using BuildersApp.Core.Repositories;
using BuildersApp.Core.Services.Interfaces;

namespace BuildersApp.Core.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserRepository _repository;
    private readonly IEncryptionService _encryptionService;
    private readonly IUserIdentityService _identityService;

    public AuthorizationService(IUserRepository repository, IEncryptionService encryptionService,
        IUserIdentityService identityService)
    {
        _repository = repository;
        _encryptionService = encryptionService;
        _identityService = identityService;
    }

    public async Task<bool> RegisterAsync(User user)
    {
        if (_repository.IsUserRegistered(user.Login))
        {
            return false;
        }

        return await _repository.CreateUser(user);
    }

    public async Task<bool> AuthorizeAsync(LoginCredentials loginCredentials)
    {
        if (!_repository.IsUserRegistered(loginCredentials.Login))
        {
            return false;
        }

        var password = _repository.GetEncryptedPasswordByLogin(loginCredentials.Login);
        var encryptedPassword = Encoding.UTF8.GetString(_encryptionService.EncryptPassword(loginCredentials.Password));
        if (!encryptedPassword.Equals(password))
            return false;
        var res = await _identityService.TrySetCurrentUserAsync(loginCredentials.Login);
        return res;
    }

    public async Task LogoutAsync()
    {
        await _identityService.LogoutAsync();
    }
}