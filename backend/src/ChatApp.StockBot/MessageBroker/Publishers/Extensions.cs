namespace ChatApp.StockBot.MessageBroker.Publishers
{
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static IServiceCollection AddPublishers(this IServiceCollection services) =>
            services
                .AddSingleton<IStockQuoteRespondedPublisher, StockQuoteRespondedPublisher>();
    }
}