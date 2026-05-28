using Microsoft.AspNetCore.Identity;

namespace TechConnect.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>()
                                        .CreateLogger("DbInitializer");

            string email = "admin@techconnect.com";
            string senha = "Admin123@";

            try
            {
                // Cria role Admin
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    logger.LogInformation("Role Admin criada.");
                }

                // Procura usuário admin
                var adminUser = await userManager.FindByEmailAsync(email);

                // Cria admin se não existir
                if (adminUser == null)
                {
                    adminUser = new IdentityUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, senha);

                    if (result.Succeeded)
                    {
                        logger.LogInformation("Usuário admin criado.");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            logger.LogError(error.Description);
                        }
                    }
                }

                // Vincula admin à role
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    logger.LogInformation("Admin vinculado à role.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao executar seed.");
            }
        }
    }
}