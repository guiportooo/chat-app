namespace ChatApp.StockBot.Amqp.AmqpIn
{
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static IServiceCollection AddAmqpIn(this IServiceCollection services) =>
            services
                .AddHostedService<StockQuoteRequestedConsumer>();
    }
}