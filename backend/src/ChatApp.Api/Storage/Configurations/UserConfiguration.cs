namespace ChatApp.Api.Storage.Configurations
{
    using Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder) =>
            builder
                .HasIndex(x => x.UserName)
                .IsUnique();
    }
}