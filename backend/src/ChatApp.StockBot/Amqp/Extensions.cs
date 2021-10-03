namespace ChatApp.StockBot.Amqp
{
    using AmqpIn;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static IServiceCollection AddAmqp(this IServiceCollection services, IConfiguration configuration) =>
            services
                .Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.Name))
                .AddAmqpIn();
    }
}