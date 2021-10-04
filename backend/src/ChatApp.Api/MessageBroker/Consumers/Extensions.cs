namespace ChatApp.Api.MessageBroker.Consumers
{
    using Microsoft.Extensions.DependencyInjection;
    using Services;

    public static class Extensions
    {
        public static IServiceCollection AddConsumers(this IServiceCollection services) =>
            services
                .AddSingleton<IStockQuoteResponseSender, StockQuoteResponseSender>()
                .AddHostedService<StockQuoteRespondedConsumer>();
    }
}