namespace ChatApp.Api.WebSocket
{
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static IServiceCollection AddWebSocket(this IServiceCollection services) =>
            services
                .AddSignalR()
                .Services;
    }
}