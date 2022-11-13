using BuildersApp.Core.Models;
using BuildersApp.Core.Repositories;
using BuildersApp.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildersApp.Data;

public static class ServiceExtension
{
    public static IServiceCollection AddData(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        var mongoDatabaseName = configuration["Data:MongoDatabase"];
        var mongoConnectionString = configuration["Data:MongoConnection"];
        var sqlConnectionString = configuration["Data:SqlConnection"];
        serviceCollection.AddScoped(_ => new MongoConnection(mongoConnectionString, mongoDatabaseName));
        serviceCollection.AddScoped(_ => new ApplicationContext(sqlConnectionString));
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IFileRepository, FileRepository>();
        serviceCollection.AddScoped<IProjectRepository, ProjectRepository>();

        return serviceCollection;
    }
}