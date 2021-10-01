namespace ChatApp.IntegrationTests
{
    using System;
    using System.IO;
    using ChatApp.Api;
    using ChatApp.Api.Storage;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
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

            using var migrationsScope = Factory.Services.CreateScope();

            var dbContext = migrationsScope.ServiceProvider.GetService<ChatAppDbContext>();

            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext), "Could not instantiate DbContext");

            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => Factory.Dispose();
    }
}