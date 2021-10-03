namespace ChatApp.Api.MessageBroker.Publishers
{
    using Domain.IntegrationEvents.Publishers;
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static IServiceCollection AddPublishers(this IServiceCollection services) =>
            services.AddSingleton<IStockQuoteRequestedPublisher, StockQuoteRequestedPublisher>();
    }
}