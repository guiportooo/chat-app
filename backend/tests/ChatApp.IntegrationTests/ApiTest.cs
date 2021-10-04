namespace ChatApp.IntegrationTests
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using AutoBogus;
    using ChatApp.Api.Domain.Models;
    using ChatApp.Api.Domain.Services;
    using ChatApp.Api.Storage;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Respawn;

    public class ApiTest
    {
        private static readonly Checkpoint ChatAppDBReset = new()
        {
            TablesToIgnore = new[] { "__EFMigrations" }
        };

        protected static IServiceScope Scope { get; private set; }
        protected static ChatAppDbContext DbContext { get; private set; }

        protected HttpClient HttpClient;

        protected static T GetService<T>() => Scope.ServiceProvider.GetService<T>();

        [SetUp]
        public async Task SetUpScope()
        {
            Scope = TestEnvironment
                .Factory
                .Services
                .CreateScope();

            DbContext = TestEnvironment
                .Factory
                .Services
                .CreateScope()
                .ServiceProvider
                .GetService<ChatAppDbContext>();

            var configuration = (ConfigurationRoot)TestEnvironment.Factory.Services.GetService(typeof(IConfiguration));
            var connectionString = configuration.GetConnectionString(ChatAppDbContext.DatabaseName);
            await ChatAppDBReset.Reset(connectionString);

            HttpClient = TestEnvironment.Factory.CreateClient();
        }

        protected void AuthorizeHttpClient()
        {
            var user = new AutoFaker<User>().Generate();
            AuthorizeHttpClient(user);
        }

        protected void AuthorizeHttpClient(User user)
        {
            var tokenGenerator = TestEnvironment.Factory.Services.GetService<ITokenGenerator>();

            if (tokenGenerator == null)
                throw new ArgumentNullException(nameof(tokenGenerator), "Could not instantiate TokenGenerator");

            var token = tokenGenerator.Generate(user);
            AuthorizeHttpClient(token);
        }

        protected void AuthorizeHttpClient(string token) =>
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        [TearDown]
        public void TearDownScope()
        {
            Scope.Dispose();
            DbContext.Dispose();
        }
    }
}