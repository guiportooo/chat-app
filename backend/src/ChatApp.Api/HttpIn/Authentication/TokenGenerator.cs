namespace ChatApp.Api.HttpIn.Authentication
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Domain.Models;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public interface ITokenGenerator
    {
        string Generate(User user);
    }

    public class TokenGenerator : ITokenGenerator
    {
        private readonly AuthenticationSettings _settings;

        public TokenGenerator(IOptions<AuthenticationSettings> settings) => _settings = settings.Value;

        public string Generate(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new(ClaimTypes.Name, user.UserName) }),
                Expires = DateTime.UtcNow.AddHours(_settings.HoursToExpire),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}