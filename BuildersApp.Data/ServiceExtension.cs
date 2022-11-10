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
        serviceCollection.AddSingleton(_ => new MongoConnection(mongoConnectionString, mongoDatabaseName));
        serviceCollection.AddSingleton(_ => new ApplicationContext(sqlConnectionString));
        serviceCollection.AddSingleton<IUserRepository, UserRepository>();
        serviceCollection.AddSingleton<IFileRepository, FileRepository>();

        return serviceCollection;
    }
}