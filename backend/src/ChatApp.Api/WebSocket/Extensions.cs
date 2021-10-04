namespace ChatApp.Api.WebSocket
{
    using Domain.IntegrationEvents.Hub;
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static IServiceCollection AddWebSocket(this IServiceCollection services) =>
            services
                .AddScoped<IHub, Hub>()
                .AddSignalR()
                .Services;
    }
}