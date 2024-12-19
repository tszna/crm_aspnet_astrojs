using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backendCRM.Models;
using BCrypt.Net;

namespace backendCRM.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var now = DateTime.UtcNow;

            builder.HasData(
                new User
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john@example.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("haslo123"),
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new User
                {
                    Id = 2,
                    Name = "John Doe1",
                    Email = "john1@example.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("haslo123"),
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new User
                {
                    Id = 3,
                    Name = "John Doe2",
                    Email = "john2@example.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("haslo123"),
                    CreatedAt = now,
                    UpdatedAt = now
                }
            );
        }
    }
}