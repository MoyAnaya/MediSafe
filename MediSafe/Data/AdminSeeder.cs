
using Microsoft.AspNetCore.Identity;

namespace MediSafe.Data;

public static class AdminSeeder
{
    public static async Task CreateAdminAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        var userManager = services.GetRequiredService<
            UserManager<IdentityUser>>();

        var roleManager = services.GetRequiredService<
            RoleManager<IdentityRole>>();

        var email = configuration["AdminSeed:Email"];
        var password = configuration["AdminSeed:Password"];

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Falta configurar AdminSeed en User Secrets.");
        }

        if (!await roleManager.RoleExistsAsync(
            ApplicationRoles.Administrator))
        {
            throw new InvalidOperationException(
                "No existe el rol Administrador.");
        }

        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(
                    $"No se pudo crear el administrador: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(
            user, ApplicationRoles.Administrator))
        {
            var result = await userManager.AddToRoleAsync(
                user, ApplicationRoles.Administrator);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(
                    $"No se pudo asignar el rol: {errors}");
            }
        }
    }
}
