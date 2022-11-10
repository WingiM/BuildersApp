﻿using BuildersApp.Core.Models;

namespace BuildersApp.Core.Services.Interfaces;

public interface IUserIdentityService
{
    public User? CurrentUser { get; }
    public Task<bool> TrySetCurrentUserAsync(string login);
    public Task<bool> TryGetCurrentUserAsync();
    public Task LogoutAsync();
}