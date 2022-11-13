using System.Text.Json;
using BuildersApp.Core.Enums;
using BuildersApp.Core.Models;
using BuildersApp.Core.Models.UserInfo;
using BuildersApp.Core.Repositories;
using BuildersApp.Core.Services.Interfaces;
using BuildersApp.Data.Models;
using Dapper;

namespace BuildersApp.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;


    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<User> GetUser(string login)
    {
        await using var session = _context.GetNpgsqlSession();
        const string sql = @"SELECT * FROM ""user"" WHERE login=@login";

        var userDb = await session.QueryFirstOrDefaultAsync<UserDb>(sql, new { login }) ??
                     throw new Exception("Not found");

        return new User
        {
            PersonalInfo = userDb.GetData(), Email = userDb.Email, Id = userDb.Id,
            Login = userDb.Login, Role = (Roles)userDb.RoleId, PhoneNumber = userDb.PhoneNumber
        };
    }

    public async Task<IEnumerable<UserTuple>> GetUsersByRole(Roles role)
    {
        await using var session = _context.GetNpgsqlSession();
        const string sql = @"SELECT id, data ->> 'Name' as ""Name"", role_id FROM ""user"" where role_id=@role_id";

        var res = await session.QueryAsync<UserTuple>(sql, new { role_id = (int)role }) ??
                  throw new Exception("Not found");

        return res;
    }

    public async Task<bool> CreateUser(User user)
    {
        var userDb = new UserDb
        {
            Data = JsonSerializer.Serialize(user.PersonalInfo), Email = user.Email, Login = user.Login,
            PhoneNumber = user.PhoneNumber, RoleId = (int)user.Role, EncryptedPassword = user.EncryptedPassword
        };

        const string sql = @"INSERT INTO ""user""(login, encrypted_password, email, phone_number, role_id, data) 
                        values (@login, @encrypted_password, @email, @phone_number, @role_id, @data::json)";
        await using var session = _context.GetNpgsqlSession();
        await session.ExecuteAsync(sql,
            new
            {
                login = userDb.Login, encrypted_password = userDb.EncryptedPassword, email = userDb.Email,
                phone_number = userDb.PhoneNumber, role_id = userDb.RoleId, data = "{}"
            });

        return true;
    }

    public string GetEncryptedPasswordByLogin(string login)
    {
        using var session = _context.GetNpgsqlSession();
        var sql = @"SELECT encrypted_password FROM ""user"" WHERE login=@login";

        return session.QueryFirstOrDefault<string>(sql, new { login }) ?? throw new Exception();
    }

    public bool IsUserRegistered(string login)
    {
        using var session = _context.GetNpgsqlSession();
        var sql = @"SELECT EXISTS(SELECT id FROM ""user"" WHERE login=@login)";

        return session.ExecuteScalar<bool>(sql, new { login });
    }

    public async Task UpdateUser(int currentUserId, PersonalInfoBase personalInfo)
    {
        await using var session = _context.GetNpgsqlSession();
        var sql = @"UPDATE ""user"" SET data = @data::json WHERE id=@userId";
        string serialized;
        switch (personalInfo)
        {
            case CustomerPersonalInfo c:
                serialized = JsonSerializer.Serialize(c);
                break;
            case DeveloperPersonalInfo dev:
                serialized = JsonSerializer.Serialize(dev);
                break;
            case DesignerPersonalInfo des:
                serialized = JsonSerializer.Serialize(des);
                break;
            default:
                throw new Exception();
        }

        await session.ExecuteAsync(sql, new { userId = currentUserId, data = serialized });
    }
}