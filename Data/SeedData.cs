using HotelAPI.Models;
using HotelAPI.Helpers;

namespace HotelAPI.Data
{
    public static class SeedData
    {
        public static void Initialize(HotelDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any()) return;

            AddUser(context, "admin@hotel.com", "admin123", "Admin");
            AddUser(context, "manager@hotel.com", "manager123", "Manager");
            AddUser(context, "user@hotel.com", "user123", "User");

            context.Rooms.AddRange(
                new Room { Number = "101", Type = "Single", Price = 50 },
                new Room { Number = "102", Type = "Double", Price = 80 },
                new Room { Number = "201", Type = "Suite", Price = 150 }
            );

            context.SaveChanges();
        }

        private static void AddUser(HotelDbContext context, string email, string password, string role)
        {
            var user = new User
            {
                Email = email,
                PasswordHash = PasswordHelper.CreatePasswordHash(password),
                Role = role
            };
            context.Users.Add(user);
        }
    }
}
