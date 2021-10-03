namespace ChatApp.Api.Amqp.AmqpOut
{
    using Domain.IntegrationEvents.Publishers;
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static IServiceCollection AddAmqpOut(this IServiceCollection services) =>
            services.AddSingleton<IStockQuoteRequestedPublisher, StockQuoteRequestedPublisher>();
    }
}