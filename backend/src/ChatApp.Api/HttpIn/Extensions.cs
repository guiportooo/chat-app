namespace ChatApp.Api.HttpIn
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Middlewares;

    public static class Extensions
    {
        public static IServiceCollection AddHttpIn(this IServiceCollection services) =>
            services
                .AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatApp.Api", Version = "v1" }))
                .AddControllers()
                .Services;

        public static IApplicationBuilder UseHttpIn(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app
                    .UseDeveloperExceptionPage()
                    .UseSwagger()
                    .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatApp.Api v1"));
            }

            return app
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthorization()
                .UseMiddleware<DomainExceptionMiddleware>()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}