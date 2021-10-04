namespace ChatApp.Api.Domain.Services
{
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services) =>
            services
                .AddScoped<IChatCommandParser, ChatCommandParser>();
    }
}