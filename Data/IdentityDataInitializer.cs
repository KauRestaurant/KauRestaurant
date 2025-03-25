using KauRestaurant.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace KauRestaurant.Data
{
    public static class IdentityDataInitializer
    {
        public static async Task SeedRolesAndAdminUser(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<KauRestaurantUser>>();

                // Create roles
                string[] roleNames = { "A1", "A2", "A3", "User" };
                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Create admin users
                var admin1Email = "admin1@example.com";
                var admin1User = await userManager.FindByEmailAsync(admin1Email);
                if (admin1User == null)
                {
                    var user = new KauRestaurantUser
                    {
                        UserName = admin1Email,
                        Email = admin1Email,
                        FirstName = "مدير",
                        LastName = "كبير",
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "A1");
                    }
                }

                // Create admin users
                var admin2Email = "admin2@example.com";
                var admin2User = await userManager.FindByEmailAsync(admin2Email);
                if (admin2User == null)
                {
                    var user = new KauRestaurantUser
                    {
                        UserName = admin2Email,
                        Email = admin2Email,
                        FirstName = "مدير",
                        LastName = "وسط",
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "A2");
                    }
                }

                // Create admin users
                var admin3Email = "admin3@example.com";
                var admin3User = await userManager.FindByEmailAsync(admin3Email);
                if (admin3User == null)
                {
                    var user = new KauRestaurantUser
                    {
                        UserName = admin3Email,
                        Email = admin3Email,
                        FirstName = "مدير",
                        LastName = "صغير",
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "A3");
                    }
                }

                // Create regular users
                var user1Email = "azizayman1421@gmail.com";
                var user1 = await userManager.FindByEmailAsync(user1Email);
                if (user1 == null)
                {
                    var user = new KauRestaurantUser
                    {
                        UserName = user1Email,
                        Email = user1Email,
                        FirstName = "أيمن",
                        LastName = "عزيز",
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "Aziz@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "User");
                    }
                }

                var user2Email = "zzozamai@gmail.com";
                var user2 = await userManager.FindByEmailAsync(user2Email);
                if (user2 == null)
                {
                    var user = new KauRestaurantUser
                    {
                        UserName = user2Email,
                        Email = user2Email,
                        FirstName = "عبدالعزيز",
                        LastName = "المالكي",
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "Almalki@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "User");
                    }
                }
            }
        }
    }
}
