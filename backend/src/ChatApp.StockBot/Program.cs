namespace ChatApp.StockBot
{
    using HttpOut;
    using MessageBroker;
    using Microsoft.Extensions.Hosting;
    using Services;

    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;

                    services
                        .AddMessageBroker(configuration)
                        .AddHttpOut(configuration)
                        .AddBotServices();
                });
    }
}