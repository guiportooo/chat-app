namespace ChatApp.IntegrationTests
{
    using System.Net.Http;
    using NUnit.Framework;

    public class ApiTest
    {
        protected HttpClient HttpClient;

        [SetUp]
        public void SetUpHttpClient() => HttpClient = TestEnvironment.Factory.CreateClient();
    }
}