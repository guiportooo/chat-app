namespace ChatApp.Api
{
    using HttpIn;
    using MediatR;
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
                .AddStorage(_configuration)
                .AddHttpIn();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) =>
            app
                .UseStorage(env)
                .UseHttpIn(env);
    }
}