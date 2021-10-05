namespace ChatApp.Api
{
    using Domain.Services;
    using HttpIn;
    using Hub;
    using MediatR;
    using MessageBroker;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Storage;

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services) =>
            services
                .AddAutoMapper(typeof(Startup))
                .AddMediatR(typeof(Startup))
                .AddDomainServices()
                .AddStorage(_configuration)
                .AddMessageBroker(_configuration)
                .AddHub()
                .AddHttpIn(_configuration);

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) =>
            app
                .UseStorage(env)
                .UseHttpIn(env);
    }
}