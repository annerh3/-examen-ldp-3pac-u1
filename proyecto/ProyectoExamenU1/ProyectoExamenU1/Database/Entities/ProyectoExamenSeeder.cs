using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoExamenU1.Constants;

namespace ProyectoExamenU1.Database.Entities
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
                    await roleManager.CreateAsync(new IdentityRole(RolesConstant.HUMAN_RES));
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

                    var normalEmployee1 = new IdentityUser // Empleados Normales
                    {
                        Email = "employee1@gmail.com",
                        UserName = "employee1@gmail.com",
                    };

                    var normalEmployee2 = new IdentityUser
                    {
                        Email = "employee2@gmail.com",
                        UserName = "employee2@gmail.com",
                    };

                    var normalEmployee3 = new IdentityUser
                    {
                        Email = "employee2@gmail.com",
                        UserName = "employee3@gmail.com",
                    };

                    await userManager.CreateAsync(userAdmin, "Temporal01*"); 
                    await userManager.CreateAsync(userHumanResources, "Temporal01*");

                    await userManager.CreateAsync(normalEmployee1, "Temporal01*"); 
                    await userManager.CreateAsync(normalEmployee2, "Temporal01*");
                    await userManager.CreateAsync(normalEmployee3, "Temporal01*");

                    await userManager.AddToRoleAsync(userAdmin, RolesConstant.ADMIN);
                    await userManager.AddToRoleAsync(userHumanResources, RolesConstant.HUMAN_RES);

                    await userManager.AddToRoleAsync(normalEmployee1, RolesConstant.EMPLOYEE);
                    await userManager.AddToRoleAsync(normalEmployee2, RolesConstant.EMPLOYEE);
                    await userManager.AddToRoleAsync(normalEmployee3, RolesConstant.EMPLOYEE);
                }

            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<ProyectoExamenSeeder>();
                logger.LogError(e.Message);
            }
        }
    }
}
