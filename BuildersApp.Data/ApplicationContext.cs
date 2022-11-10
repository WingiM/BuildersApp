using Npgsql;

namespace BuildersApp.Data;

public class ApplicationContext
{
    private readonly string _connectionString;

    public ApplicationContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Метод для подключения к базе данных
    /// </summary>
    public NpgsqlConnection GetNpgsqlSession() => new(_connectionString);
}