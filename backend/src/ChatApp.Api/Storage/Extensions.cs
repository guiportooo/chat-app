namespace ChatApp.Api.Storage
{
    using System;
    using Domain.Repositories;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Repositories;

    public static class Extensions
    {
        public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddDbContext<ChatAppDbContext>(opt =>
                    opt.UseSqlServer(configuration.GetConnectionString(ChatAppDbContext.DatabaseName)))
                .AddScoped<IUserRepository, UserRepository>();

        public static IApplicationBuilder UseStorage(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
                return app;

            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<ChatAppDbContext>();

            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext), "Could not instantiate DbContext");

            dbContext.Database.Migrate();
            return app;
        }
    }
}