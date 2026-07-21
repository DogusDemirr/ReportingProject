using CorporateReporting.Web.Constants;
using CorporateReporting.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CorporateReporting.Web.Data
{
    public class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Apply any pending migrations
            await context.Database.MigrateAsync();

            if (!await context.Roles.AnyAsync())
            {
                context.Roles.AddRange(
                    new Role { Name = RoleConstant.Admin },
                    new Role { Name = RoleConstant.Manager },
                    new Role { Name = RoleConstant.Employee }
                );
            }

            if (!await context.Departments.AnyAsync())
            {
                context.Departments.AddRange(
                    new Department { Name = DepartmentConstant.InformationTechnology },
                    new Department { Name = DepartmentConstant.Accounting },
                    new Department { Name = DepartmentConstant.Sales },
                    new Department { Name = DepartmentConstant.HumanResources }
                );
            }

            await context.SaveChangesAsync();

            if (!await context.Users.AnyAsync())
            {
                var adminRole = await context.Roles
                    .SingleAsync(x => x.Name == RoleConstant.Admin);

                var itDepartment = await context.Departments
                    .SingleAsync(x => x.Name == DepartmentConstant.InformationTechnology);

                var admin = new ApplicationUser
                {
                    FullName = "Sistem Yöneticisi",
                    Email = "admin@corporatereporting.local",
                    RoleId = adminRole.Id,
                    DepartmentId = itDepartment.Id
                };

                var passwordHasher = new PasswordHasher<ApplicationUser>();

                admin.PasswordHash = passwordHasher.HashPassword(
                    admin,
                    "Admin@12345");

                context.Users.Add(admin);

                await context.SaveChangesAsync();
            }
        }
    }
}

