using MediSafe.Components;
using MediSafe.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "No se encontró DefaultConnection.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString));

builder.Services
    .AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

Console.WriteLine("MEDISAFE: iniciando creación de roles");

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles =
    [
        ApplicationRoles.Administrator,
        ApplicationRoles.Doctor
    ];

    foreach (var role in roles)
    {
        Console.WriteLine($"MEDISAFE: comprobando rol {role}");

        if (!await roleManager.RoleExistsAsync(role))
        {
            var result = await roleManager.CreateAsync(
                new IdentityRole(role));

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    result.Errors.Select(e => e.Description));

                throw new Exception(
                    $"Error al crear {role}: {errors}");
            }

            Console.WriteLine($"MEDISAFE: rol {role} creado");
        }
        else
        {
            Console.WriteLine($"MEDISAFE: rol {role} ya existe");
        }
    }
}



app.MapPost("/login/submit", async (
    HttpContext context,
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager,
    Microsoft.AspNetCore.Antiforgery.IAntiforgery antiforgery) =>
{
    await antiforgery.ValidateRequestAsync(context);

    var form = await context.Request.ReadFormAsync();

    var email = form["email"].ToString();
    var password = form["password"].ToString();
    var rememberMe = form["rememberMe"] == "true";

    var result = await signInManager.PasswordSignInAsync(
        email,
        password,
        rememberMe,
        lockoutOnFailure: true);

    if (result.Succeeded)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            await signInManager.SignOutAsync();
            return Results.LocalRedirect("/login?error=1");
        }

        if (await userManager.IsInRoleAsync(
            user, ApplicationRoles.Administrator))
        {
            return Results.LocalRedirect("/administrador");
        }

        if (await userManager.IsInRoleAsync(
            user, ApplicationRoles.Doctor))
        {
            return Results.LocalRedirect("/medico");
        }

        await signInManager.SignOutAsync();
        return Results.LocalRedirect("/login?error=1");
    }

    return Results.LocalRedirect("/login?error=1");
});



using (var scope = app.Services.CreateScope())
{
    await AdminSeeder.CreateAdminAsync(
        scope.ServiceProvider,
        app.Configuration);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
