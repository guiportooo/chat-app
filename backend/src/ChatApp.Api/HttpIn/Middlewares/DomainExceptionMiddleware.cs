namespace ChatApp.Api.HttpIn.Middlewares
{
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Domain.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Responses;

    public class DomainExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public DomainExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (DomainException ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var error = new Error(ex.Message);
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(error, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));
            }
        }
    }
}