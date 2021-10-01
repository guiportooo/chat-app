namespace ChatApp.IntegrationTests.Api.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using AutoBogus;
    using ChatApp.Api.Domain.Models;
    using ChatApp.Api.HttpIn.Requests;
    using FluentAssertions;
    using FluentAssertions.Json;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;

    public class UsersControllerTests : ApiTest
    {
        [Test, Category("GetUser")]
        public async Task Should_return_not_found_when_user_with_informed_id_does_not_exist()
        {
            var response = await HttpClient.GetAsync("users/1");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test, Category("GetUser")]
        public async Task Should_return_ok_with_user_when_user_with_informed_id_exists()
        {
            var user = new AutoFaker<User>()
                .RuleFor(x => x.Id, () => 0)
                .Generate();

            DbContext.Users.Add(user);
            await DbContext.SaveChangesAsync();

            var response = await HttpClient.GetAsync($"users/{user.Id}");

            var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expectedContent = JToken.Parse($@"
            {{
                'id': {user.Id},
                'userName': '{user.UserName}' 
            }}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent.Should().BeEquivalentTo(expectedContent);
        }

        [Test, Category("RegisterUser")]
        public async Task Should_return_bad_request_when_user_with_informed_username_already_exists()
        {
            const string userName = "majestic.hippo";
            var errorMessage = $"Username {userName} not available";

            var user = new AutoFaker<User>()
                .RuleFor(x => x.Id, () => 0)
                .RuleFor(x => x.UserName, () => userName)
                .Generate();

            DbContext.Users.Add(user);
            await DbContext.SaveChangesAsync();

            var request = new AutoFaker<RegisterUser>()
                .RuleFor(x => x.UserName, () => userName)
                .Generate();

            var requestContent = new StringContent(JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await HttpClient.PostAsync("register", requestContent);

            var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expectedContent = JToken.Parse($@"
            {{
                'message': '{errorMessage}' 
            }}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseContent.Should().BeEquivalentTo(expectedContent);
        }

        [Test]
        public async Task Should_register_new_user()
        {
            var request = new AutoFaker<RegisterUser>().Generate();

            var requestContent = new StringContent(JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await HttpClient.PostAsync("register", requestContent);

            var userRegistered = await DbContext.Users.FirstAsync();

            var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expectedContent = JToken.Parse($@"
             {{
                 'id': {userRegistered.Id},
                 'userName': '{userRegistered.UserName}'
             }}");

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            responseContent.Should().BeEquivalentTo(expectedContent);
            response.Headers.Location?.AbsoluteUri.Should().Be($"http://localhost/users/{userRegistered.Id}");
        }
    }
}