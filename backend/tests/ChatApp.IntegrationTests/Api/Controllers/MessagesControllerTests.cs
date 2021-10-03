namespace ChatApp.IntegrationTests.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using AutoBogus;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using Requests = ChatApp.Api.HttpIn.Requests;
    using Responses = ChatApp.Api.HttpIn.Responses;
    using Models = ChatApp.Api.Domain.Models;

    public class MessagesControllerTests : ApiTest
    {
        [Test, Category("SendMessage")]
        public async Task Should_return_bad_request_when_room_with_code_to_send_message_does_not_exist()
        {
            const string roomCode = "ghost-room";
            var errorMessage = $"Room {roomCode} does not exist";

            var request = new AutoFaker<Requests.SendMessage>().Generate();

            var requestContent = new StringContent(JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            AuthorizeHttpClient();
            var response = await HttpClient.PostAsync($"rooms/{roomCode}/messages", requestContent);

            var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expectedContent = JToken.Parse($@"
            {{
                'message': '{errorMessage}' 
            }}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseContent.Should().BeEquivalentTo(expectedContent);
        }

        [Test, Category("SendMessage")]
        public async Task Should_return_bad_request_when_user_with_username_does_not_exist()
        {
            const string userName = "ghost-user";
            var errorMessage = $"User {userName} does not exist";

            var room = new AutoFaker<Models.Room>()
                .RuleFor(x => x.Id, () => 0)
                .RuleFor(x => x.Messages, () => new List<Models.Message>())
                .Generate();

            DbContext.Rooms.Add(room);
            await DbContext.SaveChangesAsync();

            var request = new AutoFaker<Requests.SendMessage>().Generate();

            var requestContent = new StringContent(JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            AuthorizeHttpClient();
            var response = await HttpClient.PostAsync($"rooms/{room.Code}/messages", requestContent);

            var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expectedContent = JToken.Parse($@"
                    {{
                        'message': '{errorMessage}' 
                    }}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseContent.Should().BeEquivalentTo(expectedContent);
        }

        [Test, Category("SendMessage")]
        public async Task Should_send_message()
        {
            var user = new AutoFaker<Models.User>()
                .RuleFor(x => x.Id, () => 0)
                .Generate();

            var room = new AutoFaker<Models.Room>()
                .RuleFor(x => x.Id, () => 0)
                .RuleFor(x => x.Code, () => "general")
                .RuleFor(x => x.Messages, () => new List<Models.Message>())
                .Generate();

            DbContext.Users.Add(user);
            DbContext.Rooms.Add(room);
            await DbContext.SaveChangesAsync();

            var request = new AutoFaker<Requests.SendMessage>().Generate();

            var requestContent = new StringContent(JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            AuthorizeHttpClient(user);
            var response = await HttpClient.PostAsync($"rooms/{room.Code}/messages", requestContent);

            var messageSent = await DbContext.Messages.FirstAsync();

            var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expectedContent = JToken.Parse($@"
            {{
                'id': {messageSent.Id},
                'text': '{messageSent.Text}',
                'timestamp': '{messageSent.Timestamp}',
                'userName': '{user.UserName}',
                'roomCode': '{room.Code}'
            }}");

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            responseContent.Should().BeEquivalentTo(expectedContent);
            response.Headers.Location?.AbsoluteUri.Should()
                .Be($"http://localhost/rooms/{room.Code}/Messages/{messageSent.Id}");
        }

        [Test, Category("SendMessage")]
        public async Task Should_send_stock_command()
        {
            var user = new AutoFaker<Models.User>()
                .RuleFor(x => x.Id, () => 0)
                .Generate();

            var room = new AutoFaker<Models.Room>()
                .RuleFor(x => x.Id, () => 0)
                .RuleFor(x => x.Code, () => "general")
                .RuleFor(x => x.Messages, () => new List<Models.Message>())
                .Generate();

            DbContext.Users.Add(user);
            DbContext.Rooms.Add(room);
            await DbContext.SaveChangesAsync();

            var request = new AutoFaker<Requests.SendMessage>()
                .RuleFor(x => x.Text, () => "/stock=AAPL.US")
                .Generate();

            var requestContent = new StringContent(JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            AuthorizeHttpClient(user);
            var response = await HttpClient.PostAsync($"rooms/{room.Code}/messages", requestContent);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Test, Category("GetMessages")]
        public async Task Should_return_bad_request_when_room_with_code_to_get_messages_from_does_not_exist()
        {
            const string roomCode = "ghost-room";
            var errorMessage = $"Room {roomCode} does not exist";

            AuthorizeHttpClient();
            var response = await HttpClient.GetAsync($"rooms/{roomCode}/messages");

            var responseContent = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expectedContent = JToken.Parse($@"
            {{
                'message': '{errorMessage}' 
            }}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseContent.Should().BeEquivalentTo(expectedContent);
        }

        [Test, Category("GetMessages")]
        public async Task Should_return_no_content_when_room_with_code_does_not_have_messages()
        {
            var room = new AutoFaker<Models.Room>()
                .RuleFor(x => x.Id, () => 0)
                .RuleFor(x => x.Messages, () => new List<Models.Message>())
                .Generate();

            DbContext.Rooms.Add(room);
            await DbContext.SaveChangesAsync();

            AuthorizeHttpClient();
            var response = await HttpClient.GetAsync($"rooms/{room.Code}/messages");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Test, Category("GetMessages")]
        public async Task Should_return_last_fifty_messages_from_room_with_code()
        {
            var user = new AutoFaker<Models.User>()
                .RuleFor(x => x.Id, () => 0)
                .Generate();

            var messages = new AutoFaker<Models.Message>()
                .RuleFor(x => x.Id, () => 0)
                .RuleFor(x => x.RoomId, () => 0)
                .RuleFor(x => x.Room, () => null)
                .RuleFor(x => x.UserId, () => user.Id)
                .RuleFor(x => x.User, () => user)
                .Generate(51);

            var room = new AutoFaker<Models.Room>()
                .RuleFor(x => x.Id, () => 0)
                .RuleFor(x => x.Messages, () => messages)
                .Generate();

            DbContext.Rooms.Add(room);
            await DbContext.SaveChangesAsync();

            AuthorizeHttpClient();
            var response = await HttpClient.GetAsync($"rooms/{room.Code}/messages");

            var responseContent = await response.Content.ReadAsStringAsync();

            var receivedMessages =
                JsonSerializer.Deserialize<Responses.MessagesSent>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            var messageToExclude = messages.OrderBy(x => x.Timestamp).First();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            receivedMessages?.Messages.Count().Should().Be(50);
            receivedMessages?.Messages.Any(x => x.Id == messageToExclude.Id).Should().BeFalse();
        }
    }
}