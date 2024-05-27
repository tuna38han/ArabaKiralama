using Microsoft.AspNetCore.Identity;

namespace ArabaKirala.API.Models;

public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        string adminRole = "Admin";
        string userRole = "User";

        foreach (var roleName in new[] { adminRole, userRole})
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                {
                    throw new ApplicationException($"The role could not be created: {roleName}");
                }
            }
        }

        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser == null)
        {
            var newAdminUser = new AppUser
            {
                FirstName = "admin",
                LastName = "admin",
                UserName = "admin",
                Email = "admin@example.com",
                PhoneNumber = "11111111",
                Role = "Admin"
            };
            await userManager.CreateAsync(newAdminUser, "Admin1.");
            await userManager.AddToRoleAsync(newAdminUser, adminRole);
        }
        
    }
}