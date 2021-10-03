namespace ChatApp.StockBot.MessageBroker
{
    using Consumers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Publishers;

    public static class Extensions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.Name))
                .AddConsumers()
                .AddPublishers();
    }
}