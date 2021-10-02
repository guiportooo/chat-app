namespace ChatApp.Api.Storage
{
    using System;
    using System.Linq;
    using Domain.Models;
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
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IRoomRepository, RoomRepository>()
                .AddScoped<IMessageRepository, MessageRepository>();

        public static IApplicationBuilder UseStorage(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
                return app;

            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<ChatAppDbContext>();

            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext), "Could not instantiate DbContext");

            dbContext.Database.Migrate();

            if (dbContext.Rooms.Any())
                return app;

            dbContext.Rooms.Add(new Room("general"));
            dbContext.SaveChanges();
            return app;
        }
    }
}