namespace ChatApp.Api.HttpIn
{
    using Authentication;
    using Hub;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Middlewares;

    public static class Extensions
    {
        public static IServiceCollection AddHttpIn(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatApp.Api", Version = "v1" });
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            System.Array.Empty<string>()
                        }
                    });
                })
                .AddBearerAuthentication(configuration)
                .AddCors(options =>
                {
                    options.AddPolicy("ClientPermission", policy =>
                    {
                        policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithOrigins("http://localhost:3000")
                            .AllowCredentials();
                    });
                })
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
                .UseCors("ClientPermission")
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseMiddleware<DomainExceptionMiddleware>()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<SignalRHub>("/hubs/chat");
                });
        }
    }
}