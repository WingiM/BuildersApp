namespace BuildersApp.Core.Services.Interfaces;

public interface IEncryptionService
{
    public byte[] EncryptPassword(string password);
    public string GetJwtForUser(string login);
}