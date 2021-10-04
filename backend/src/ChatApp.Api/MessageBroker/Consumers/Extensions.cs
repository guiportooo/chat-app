namespace ChatApp.Api.MessageBroker.Consumers
{
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static IServiceCollection AddConsumers(this IServiceCollection services) =>
            services
                .AddHostedService<StockQuoteRespondedConsumer>();
    }
}