using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrasitructure
{
    public static class DbInitalizer
    {
        public static async Task SeedAdminData(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // admin data
            string adminName = "admin";
            string adminEmail = "admin@gmail.com";
            string adminPassword = "Admin#123";
            string adminRole = "Admin";


            // check if role exists
            if (!await roleManager.RoleExistsAsync(adminRole))
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            // check if role exists
            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser is null)
            {
                var user = new User
                {
                    Email = adminEmail,
                    UserName = adminName,
                    FullName = "Admin User",
                    NormalizedEmail = adminEmail.ToUpper(),
                    EmailConfirmed = true,
                };
                var result = await userManager.CreateAsync(user, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, adminRole);
                }
            }
        }


    }
}
