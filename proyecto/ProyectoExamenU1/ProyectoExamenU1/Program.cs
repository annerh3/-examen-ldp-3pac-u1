using ProyectoExamenU1;
var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
var app = builder.Build(); //  Construye la aplicaci�n.
startup.Configure(app, app.Environment);
app.Run();
