using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BestReg.Data
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Apply any pending migrations and create the database if needed
            await context.Database.MigrateAsync();

            // Seed roles
            if (!roleManager.Roles.Any())
            {
                await SeedRolesAsync(roleManager);
            }

            // Seed admin user
            if (!userManager.Users.Any(u => u.UserName == "admin@gmail.com"))
            {
                await SeedAdminUserAsync(userManager, roleManager);
            }
        }
        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "SchoolSecurity", "BusDriver", "Student", "Parent" };
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var users = new List<(ApplicationUser User, string Password, string Role)>
        {
            (new ApplicationUser { UserName = "admin@example.com", Email = "admin@example.com", EmailConfirmed = true }, "Admin@123", "Admin"),
            (new ApplicationUser { UserName = "security@example.com", Email = "security@example.com", EmailConfirmed = true }, "Security@123", "SchoolSecurity"),
            (new ApplicationUser { UserName = "driver@example.com", Email = "driver@example.com", EmailConfirmed = true }, "Driver@123", "BusDriver"),
            (new ApplicationUser { UserName = "student@example.com", Email = "student@example.com", EmailConfirmed = true }, "Student@123", "Student"),
            (new ApplicationUser { UserName = "parent@example.com", Email = "parent@example.com", EmailConfirmed = true }, "Parent@123", "Parent")
        };

            foreach (var (user, password, role) in users)
            {
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, role);
            }
        }


        private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Create admin user
            var adminUser = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                FirstName = "Admin",
                LastName = "User",
                IDNumber = "123456789012",
                LockoutEnabled = false,  // Ensure the account is not locked out
                EmailConfirmed = true, // Skip email confirmation for admin
                QrCodeBase64 = null
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@1234");
            if (result.Succeeded)
            {
                Console.WriteLine("Admin user created successfully.");

                // Check if the Admin role exists
                if (await roleManager.RoleExistsAsync("Admin"))
                {
                    // Assign admin role
                    var addToRoleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (addToRoleResult.Succeeded)
                    {
                        Console.WriteLine("Admin user added to Admin role successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Error adding admin user to Admin role: {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    Console.WriteLine("Admin role does not exist. Admin user was created but not assigned to any role.");
                }
            }
            else
            {
                // Log error messages using your logging framework
                Console.WriteLine($"Error creating admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

    }
}
