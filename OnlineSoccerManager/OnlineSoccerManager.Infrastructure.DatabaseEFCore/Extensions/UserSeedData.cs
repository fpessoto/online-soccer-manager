using Microsoft.EntityFrameworkCore;
using OnlineSoccerManager.Domain.Users;

namespace OnlineSoccerManager.Infrastructure.DatabaseEFCore.Extensions
{
    public static class UserSeedData
    {
        public static void SeedUser(this ModelBuilder builder)
        {
            var user = new User
            {
                Id = Guid.Parse("4db89c05-1b67-4771-96a2-4b607d287123"),
                Email = "admin@admin.com",
                Password = "admin",
                Role = "admin",
                Username = "admin",
                CreatedDate = new DateTime(2022, 3, 30, 16, 14, 37, 755, DateTimeKind.Local).AddTicks(4636),
                UpdatedDate = new DateTime(2022, 3, 30, 16, 14, 37, 758, DateTimeKind.Local).AddTicks(1509)
            };
            builder.Entity<User>().HasData(user);
        }
    }
}