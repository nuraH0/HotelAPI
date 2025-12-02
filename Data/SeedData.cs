using HotelAPI.Data;
using HotelAPI.Helpers;

public static class SeedData
{
    public static void Initialize(HotelDbContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Users.Any())
        {
            AddUser(context, "admin@hotel.com", "Admin123!", "Admin");
            AddUser(context, "manager@hotel.com", "Manager123!", "Manager");
            AddUser(context, "user@hotel.com", "User123!", "User");

            context.SaveChanges();
        }
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
