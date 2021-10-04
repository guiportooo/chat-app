namespace ChatApp.Api.HttpIn.Authentication
{
    using System.Text;
    using Domain.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    public static class Extensions
    {
        public static IServiceCollection AddBearerAuthentication(this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .Configure<AuthenticationSettings>(configuration.GetSection(AuthenticationSettings.Name))
                .AddScoped<ITokenGenerator, TokenGenerator>()
                .AddScoped<IPasswordHasher, PasswordHasher>()
                .AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    var secret = Encoding.ASCII.GetBytes(configuration
                        .GetSection(AuthenticationSettings.Name)
                        .GetValue<string>("Secret"));

                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                })
                .Services;
    }
}