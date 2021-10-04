namespace ChatApp.IntegrationTests.Api.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using AutoBogus;
    using ChatApp.Api.Domain.Services;
    using FluentAssertions;
    using FluentAssertions.Json;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using Requests = ChatApp.Api.HttpIn.Requests;
    using Responses = ChatApp.Api.HttpIn.Responses;
    using Models = ChatApp.Api.Domain.Models;

    public class UsersControllerTests : ApiTest
    {
        [TestCase(null), Category("GetUser")]
        [TestCase(""), Category("GetUser")]
        [TestCase("invalid.token"), Category("GetUser")]
        public async Task Should_return_unauthorized_when_calling_without_a_valid_token(string token)
        {
            if (token is not null)
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await HttpClient.GetAsync("users/1");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test, Category("GetUser")]
        public async Task Should_return_not_found_when_user_with_informed_id_does_not_exist()
        {
            AuthorizeHttpClient();
            var response = await HttpClient.GetAsync("users/1");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test, Category("GetUser")]
        public async Task Should_return_ok_with_user_when_user_with_informed_id_exists()
        {
            var user = new AutoFaker<Models.User>()
                .RuleFor(x => x.Id, () => 0)
                .Generate();

            DbContext.Users.Add(user);
            await DbContext.SaveChangesAsync();

            AuthorizeHttpClient();
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

            var user = new AutoFaker<Models.User>()
                .RuleFor(x => x.Id, () => 0)
                .RuleFor(x => x.UserName, () => userName)
                .Generate();

            DbContext.Users.Add(user);
            await DbContext.SaveChangesAsync();

            var request = new AutoFaker<Requests.RegisterUser>()
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

        [Test, Category("RegisterUser")]
        public async Task Should_register_new_user()
        {
            var request = new AutoFaker<Requests.RegisterUser>().Generate();

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

        [TestCase("majestic.hippo", "incorrect.password"), Category("AuthenticateUser")]
        [TestCase("incorrect.username", "pass@123"), Category("AuthenticateUser")]
        public async Task Should_return_unauthorized_when_users_information_is_invalid(string userName, string password)
        {
            var passwordHasher = Scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
            const string correctPassword = "pass@123";
            var protectedPassword = passwordHasher.Hash(correctPassword);
            
            var user = new AutoFaker<Models.User>()
                .RuleFor(x => x.Id, () => 0)
                .RuleFor(x => x.UserName, () => "majestic.hippo")
                .RuleFor(x => x.Password, () => protectedPassword)
                .Generate();

            DbContext.Users.Add(user);
            await DbContext.SaveChangesAsync();

            var request = new AutoFaker<Requests.AuthenticateUser>()
                .RuleFor(x => x.UserName, () => userName)
                .RuleFor(x => x.Password, () => password)
                .Generate();

            var requestContent = new StringContent(JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await HttpClient.PostAsync("authenticate", requestContent);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test, Category("AuthenticateUser")]
        public async Task Should_return_valid_token_when_users_information_is_valid()
        {
            var passwordHasher = Scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
            const string password = "pass@123";
            var protectedPassword = passwordHasher.Hash(password);
            
            var user = new AutoFaker<Models.User>()
                .RuleFor(x => x.Id, () => 0)
                .RuleFor(x => x.Password, () => protectedPassword)
                .Generate();

            DbContext.Users.Add(user);
            await DbContext.SaveChangesAsync();

            var authRequest = new AutoFaker<Requests.AuthenticateUser>()
                .RuleFor(x => x.UserName, () => user.UserName)
                .RuleFor(x => x.Password, () => password)
                .Generate();

            var authRequestContent = new StringContent(JsonSerializer.Serialize(authRequest),
                Encoding.UTF8,
                "application/json");

            var authResponse = await HttpClient.PostAsync("authenticate", authRequestContent);

            var authResponseContent = await authResponse.Content.ReadAsStringAsync();
            var authenticatedUser = JsonSerializer.Deserialize<Responses.AuthenticatedUser>(authResponseContent,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            AuthorizeHttpClient(authenticatedUser?.Token);
            var getResponse = await HttpClient.GetAsync($"users/{user.Id}");

            var getResponseContent = JToken.Parse(await getResponse.Content.ReadAsStringAsync());
            var expectedContent = JToken.Parse($@"
            {{
                'id': {user.Id},
                'userName': '{user.UserName}' 
            }}");

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            getResponseContent.Should().BeEquivalentTo(expectedContent);
        }
    }
}