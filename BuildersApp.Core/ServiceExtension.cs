using BuildersApp.Core.Services;
using BuildersApp.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BuildersApp.Core;

public static class ServiceExtension
{
    public static IServiceCollection AddCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IEncryptionService, EncryptionService>();
        serviceCollection.AddScoped<ILocalStorageService, LocalStorageService>();
        serviceCollection.AddScoped<IUserIdentityService, UserIdentityService>();
        serviceCollection.AddScoped<IAuthorizationService, AuthorizationService>();
        return serviceCollection;
    }
}