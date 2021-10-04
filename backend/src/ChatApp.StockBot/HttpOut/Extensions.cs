namespace ChatApp.StockBot.HttpOut
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static IServiceCollection AddHttpOut(this IServiceCollection services, IConfiguration configuration) =>
            services
                .Configure<StooqHttpClientSettings>(configuration.GetSection(StooqHttpClientSettings.Name))
                .AddHttpClient<IStooqHttpClient, StooqHttpClient>(client =>
                    client.BaseAddress = new Uri(configuration
                        .GetSection("StooqHttpClient")
                        .GetValue<string>("Host")))
                .Services;
    }
}