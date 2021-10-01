namespace ChatApp.IntegrationTests
{
    using System.IO;
    using ChatApp.Api;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using NUnit.Framework;

    [SetUpFixture]
    public class TestEnvironment
    {
        public static WebApplicationFactory<Startup> Factory { get; private set; }
        public static IConfigurationRoot Configuration { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder
                        .ConfigureTestServices(_ => 
                        {
                            //Configure mocked services
                        })
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseEnvironment("Test");
                });

            Configuration = new ConfigurationBuilder()
                .SetBasePath(TestContext.CurrentContext.TestDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => Factory.Dispose();
    }
}