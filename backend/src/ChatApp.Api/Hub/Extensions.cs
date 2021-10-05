namespace ChatApp.Api.Hub
{
    using Domain.IntegrationEvents.Hub;
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static IServiceCollection AddHub(this IServiceCollection services) =>
            services
                .AddScoped<IHub, ChatHub>()
                .AddSignalR()
                .Services;
    }
}