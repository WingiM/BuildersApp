using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using BuildersApp.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BuildersApp.Core.Services;

public class EncryptionService : IEncryptionService
{
    private readonly IConfiguration _configuration;
    private readonly HMACSHA256 _encryptor;

    public EncryptionService(IConfiguration configuration)
    {
        _configuration = configuration;
        _encryptor = new HMACSHA256(
            Encoding.UTF8.GetBytes(configuration["Encryption:AnalogKey"]));
    }

    public byte[] EncryptPassword(string password)
    {
        return _encryptor.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public string GetJwtForUser(string login)
    {
        var jwt = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            login,
            expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256)
        );

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }
}