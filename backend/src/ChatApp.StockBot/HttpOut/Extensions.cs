namespace ChatApp.StockBot.HttpOut
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static IServiceCollection AddHttpOut(this IServiceCollection services, IConfiguration configuration) =>
            services
                .Configure<StooqHttpClientSettings>(configuration.GetSection(StooqHttpClientSettings.Name))
                .AddSingleton<IStooqHttpClient, StooqHttpClient>();
    }
}