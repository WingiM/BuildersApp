using System.IdentityModel.Tokens.Jwt;
using BuildersApp.Core.Enums;
using BuildersApp.Core.Models.UserInfo;
using BuildersApp.Core.Repositories;
using BuildersApp.Core.Services.Interfaces;

namespace BuildersApp.Core.Services;

public class UserIdentityService : IUserIdentityService
{
    private readonly IEncryptionService _encryptionService;
    private readonly IUserRepository _userRepository;
    private readonly ILocalStorageService _localStorageService;

    public UserIdentityService(ILocalStorageService localStorageService, IUserRepository userRepository,
        IEncryptionService encryptionService)
    {
        _localStorageService = localStorageService;
        _userRepository = userRepository;
        _encryptionService = encryptionService;
    }

    public User? CurrentUser { get; private set; }

    public async Task<bool> TrySetCurrentUserAsync(string login)
    {
        var jwt = _encryptionService.GetJwtForUser(login);
        await _localStorageService.SetStringAsync(LocalStorageKeys.Authorization, jwt);
        try
        {
            CurrentUser = await _userRepository.GetUser(login);
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> TryGetCurrentUserAsync()
    {
        var jwt = await _localStorageService.GetStringAsync(LocalStorageKeys.Authorization);
        if (jwt is null)
            return false;
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(jwt);

        CurrentUser = await _userRepository.GetUser(jwtSecurityToken.Audiences.First());
        return true;
    }

    public async Task LogoutAsync()
    {
        await _localStorageService.RemoveAsync(LocalStorageKeys.Authorization);
        CurrentUser = null;
    }

    public async Task UpdateUser(int currentUserId, PersonalInfoBase personalInfo)
    {
        await _userRepository.UpdateUser(currentUserId, personalInfo);
        await TryGetCurrentUserAsync();
    }
}