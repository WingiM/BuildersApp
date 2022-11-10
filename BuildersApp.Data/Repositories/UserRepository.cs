﻿using System.Text.Json;
using BuildersApp.Core.Enums;
using BuildersApp.Core.Models.UserInfo;
using BuildersApp.Core.Repositories;
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
            PersonalInfoBase = userDb.GetData(), Email = userDb.Email,
            Login = userDb.Login, Role = (Roles)userDb.RoleId
        };
    }

    public async Task<bool> CreateUser(User user)
    {
        var userDb = new UserDb
        {
            Data = JsonSerializer.Serialize(user.PersonalInfoBase), Email = user.Email, Login = user.Login,
            PhoneNumber = user.PhoneNumber, RoleId = (int)user.Role, EncryptedPassword = user.EncryptedPassword
        };

        const string sql = @"INSERT INTO ""user""(login, encrypted_password, email, phone_number, role_id, data) 
                        values (@login, @encrypted_password, @email, @phone_number, @role_id, @data::json)";
        await using var session = _context.GetNpgsqlSession();
        await session.ExecuteAsync(sql,
            new
            {
                login = userDb.Login, encrypted_password = userDb.EncryptedPassword, email = userDb.Email,
                phone_number = userDb.PhoneNumber, role_id = userDb.RoleId, data = userDb.Data
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
}