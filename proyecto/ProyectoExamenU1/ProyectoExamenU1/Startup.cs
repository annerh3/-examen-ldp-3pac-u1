using Microsoft.EntityFrameworkCore;
using ProyectoExamenU1.Database;

namespace ProyectoExamenU1
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers().AddNewtonsoftJson(options => // Añadir Controladores con Newtonsoft.Json (del pack: Microsoft.AspNetCore.Mvc.NewtonsoftJson)
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            // Add DbContext
            services.AddDbContext<ProyectoExamenContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Add custom services

            //services.AddTransient<IPrestamoService, PrestamoService>();


            // Add AutoMapper

            // services.AddAutoMapper(typeof(AutoMapperProfile));
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}