namespace ChatApp.StockBot.Services
{
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static IServiceCollection AddBotServices(this IServiceCollection services) =>
            services
                .AddSingleton<IStockQuoteCsvParser, StockQuoteParser>()
                .AddSingleton<IStockQuoteService, StockQuoteService>();
    }
}