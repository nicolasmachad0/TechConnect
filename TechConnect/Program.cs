using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechConnect.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."),
        sqlOptions => sqlOptions.EnableRetryOnFailure() // resiliência para falhas transitórias
    ));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // necessário para as páginas de login do Identity

// Seed do usuário admin (com migração e tratamento de erros)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // Aplica migrações pendentes (cria banco se necessário)
        await context.Database.MigrateAsync();

        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Cria a role Admin se não existir
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        string email = "admin@techconnect.com";
        string senha = "Admin123@";

        if (await userManager.FindByEmailAsync(email) == null)
        {
            var admin = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, senha);
        }

        // Atribui a role Admin ao usuário (verifica null)
        var adminUser = await userManager.FindByEmailAsync(email);
        if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro ao aplicar migrações / seed do usuário admin.");
        // opcional: rethrow se quiser que a aplicação falhe ao iniciar
        // throw;
    }
}

app.Run();