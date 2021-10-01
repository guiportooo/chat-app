namespace ChatApp.IntegrationTests.Api.Controllers
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using NUnit.Framework;

    public class HelloWorldControllerTests : ApiTest
    {
        [Test]
        public async Task Should_return_ok_with_hello_world_content()
        {
            var response = await HttpClient.GetAsync("helloworld");
            var responseContent = await response.Content.ReadAsStringAsync();
            const string expectedContent = "Hello World!";
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            responseContent.Should().Be(expectedContent);
        }
    }
}