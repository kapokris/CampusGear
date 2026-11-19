using Microsoft.AspNetCore.Identity;

namespace CampusGear.Web.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = { "Student", "Staff", "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            await AddUser(userManager, "student@campusgear.com", "Student");
            await AddUser(userManager, "staff@campusgear.com", "Staff");
            await AddUser(userManager, "admin@campusgear.com", "Admin");
        }

        private static async Task AddUser(UserManager<IdentityUser> userManager, string email, string role)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser();
                user.UserName = email;
                user.Email = email;
                user.EmailConfirmed = true;

                await userManager.CreateAsync(user, "Campus123!");
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
