using Microsoft.AspNetCore.Identity;

namespace SciencePortalMVC.Data
{
    public static class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // --- Создание ролей ---
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (await roleManager.FindByNameAsync("User") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // --- Создание пользователя-администратора ---
            string adminEmail = "admin@portal.com";
            string adminPassword = "AdminPassword123!"; // Пароль должен быть сложным

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                IdentityUser admin = new IdentityUser
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true // Сразу подтверждаем почту
                };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    // Присваиваем роль "Admin"
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}