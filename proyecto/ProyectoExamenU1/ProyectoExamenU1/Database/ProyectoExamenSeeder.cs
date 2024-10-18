using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProyectoExamenU1.Constants;
using ProyectoExamenU1.Database.Entities;
using ProyectoExamenU1.Services.Interfaces;

namespace ProyectoExamenU1.Database
{
    public class ProyectoExamenSeeder
    {

        public static async Task LoadDataAsync(
            ProyectoExamenContext context,
            ILoggerFactory loggerFactory,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            try
            {
                await LoadRolesAndUSersAsync(userManager, roleManager, loggerFactory);
                await LoadPermitionTypesAsync(loggerFactory, context, userManager);

            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<ProyectoExamenSeeder>();
                logger.LogError(e, "Error inicializando la data del API");
            }
        }




        public static async Task LoadRolesAndUSersAsync(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILoggerFactory loggerFactory)
        {
            try
            {
                if (!await roleManager.Roles.AnyAsync())
                {
                    //Evitamos tener los string quemados
                    await roleManager.CreateAsync(new IdentityRole(RolesConstant.ADMIN));
                    await roleManager.CreateAsync(new IdentityRole(RolesConstant.HUMAN_RESOURCES));
                    await roleManager.CreateAsync(new IdentityRole(RolesConstant.EMPLOYEE));
                }

                if (!await userManager.Users.AnyAsync())
                {
                    var userAdmin = new IdentityUser
                    {
                        Email = "admin@gmail.com",
                        UserName = "admin@gmail.com",
                    };

                    var userHumanResources = new IdentityUser // Human Resources
                    {
                        Email = "human_r@gmail.com",
                        UserName = "human_r@gmail.com",
                    };


                    int employeeAmount = 5; // cantidad de empleados

                    List<IdentityUser> employees = new List<IdentityUser>();

                    //para crear n cantidad de empleados.  Anner
                    for (int i = 1; i <= employeeAmount; i++)
                    {
                        var employee = new IdentityUser
                        {
                            Email = $"employee{i}@gmail.com",
                            UserName = $"employee{i}@gmail.com"
                        };

                        employees.Add(employee);
                        await userManager.CreateAsync(employee, "Temporal01*");
                    }

                    await userManager.CreateAsync(userAdmin, "Temporal01*");
                    await userManager.CreateAsync(userHumanResources, "Temporal01*");



                    await userManager.AddToRoleAsync(userAdmin, RolesConstant.ADMIN);
                    await userManager.AddToRoleAsync(userHumanResources, RolesConstant.HUMAN_RESOURCES);

                    // Asignar rol a los empleados en la variable empleados 
                    foreach (var employee in employees)
                    {
                        await userManager.AddToRoleAsync(employee, RolesConstant.EMPLOYEE);
                    }
                }

            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<ProyectoExamenSeeder>();
                logger.LogError(e.Message);
            }
        }

        public static async Task LoadPermitionTypesAsync(ILoggerFactory loggerFactory, ProyectoExamenContext context, UserManager<IdentityUser> userManager)
        {
            try
            {
                var jsonFilePath = "SeedData/permitionTypes.json";
                var jsonContent = await File.ReadAllTextAsync(jsonFilePath);
                var categories = JsonConvert.DeserializeObject<List<PermitionTypeEntity>>(jsonContent);

                if (!await context.PermitionTypes.AnyAsync())
                {
                    var user = await userManager.Users.FirstOrDefaultAsync(); ;
                    if (user == null)
                    {
                        user = new IdentityUser
                        {
                            UserName = "SeederUser",
                            Email = "seeder@example.com"
                        };
                        var result = await userManager.CreateAsync(user, "SeederPassword123!");
                        await userManager.AddToRoleAsync(user, RolesConstant.ADMIN);

                        if (!result.Succeeded)
                        {
                            throw new Exception("fallo al crear el usuario de semilla");
                        }
                    }

                    for (int i = 0; i < categories.Count; i++)
                    {
                        categories[i].CreatedBy = user.Id;
                        categories[i].CreatedDate = DateTime.Now;
                        categories[i].UpdatedBy = user.Id;
                        categories[i].UpdatedDate = DateTime.Now;
                    }

                    context.AddRange(categories);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<ProyectoExamenSeeder>();
                logger.LogError(e, "Error al ejecutar el Seed de permitionTypes");
            }
        }
    }
}
