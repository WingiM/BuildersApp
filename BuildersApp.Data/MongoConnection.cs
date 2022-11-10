using MongoDB.Driver;

namespace BuildersApp.Data;

public class MongoConnection
{
    private MongoClient? Client { get; }
    internal IMongoDatabase? Database { get; }

    public MongoConnection(string connectionString, string databaseName)
    {
        Client = new MongoClient(connectionString);
        Database = Client.GetDatabase(databaseName);
    }
}