using Microsoft.AspNetCore.Identity;
using ProyectoExamenU1;
using ProyectoExamenU1.Database;
using ProyectoExamenU1.Services.Interfaces;
var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try
    {
        var context = services.GetRequiredService<ProyectoExamenContext>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var auditService = services.GetRequiredService<IAuditService>();

        await ProyectoExamenSeeder.LoadDataAsync(context, loggerFactory, userManager, roleManager,auditService );
    }
    catch (Exception e)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(e, "Error al ejecutar el Seed de datos");
    }

}

app.Run();