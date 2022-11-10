using BuildersApp.Core.Models.UserInfo;

namespace BuildersApp.Core.Repositories;

public interface IUserRepository
{
    public Task<User> GetUser(string login);
    public Task<bool> CreateUser(User user);
    public string GetEncryptedPasswordByLogin(string login);
    public bool IsUserRegistered(string login);
}