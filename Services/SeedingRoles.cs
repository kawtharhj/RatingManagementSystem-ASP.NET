using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RatingManagementSystem.Data;

namespace RatingManagementSystem.Services
{
    public class SeedingRoles
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            using (var context = new RatingSystemDbContext(serviceProvider.GetRequiredService<DbContextOptions<RatingSystemDbContext>>()))
            {
                // Define the roles you want to seed
                string[] roles = new string[] { "Admin", "Employee", "Doctor" };

                // Check for existing roles to avoid duplicates
                var existingRoles = await context.Roles.ToListAsync();

                foreach (var role in roles)
                {
                    if (!existingRoles.Any(r => r.Name == role))
                    {

                        context.Roles.Add(new IdentityRole(role));
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
