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
            #region Satış tablo verilerini oluşturur
            if (!await context.SalesData.AnyAsync())
            {
                var salesDepartment = await context.Departments
                    .SingleAsync(x => x.Name == "Satış");

                var today = DateTime.UtcNow.Date;

                context.SalesData.AddRange(
                    new SalesData
                    {
                        OrderNumber = "SLS-1001",
                        SaleDate = today.AddDays(-1),
                        CustomerName = "Atlas Teknoloji A.Ş.",
                        ProductName = "Kurumsal Lisans Paketi",
                        Category = "Yazılım",
                        Quantity = 5,
                        UnitPrice = 12500m,
                        TotalAmount = 62500m,
                        DepartmentId = salesDepartment.Id
                    },
                    new SalesData
                    {
                        OrderNumber = "SLS-1002",
                        SaleDate = today.AddDays(-3),
                        CustomerName = "Mavi Grup",
                        ProductName = "Analiz Modülü",
                        Category = "Yazılım",
                        Quantity = 8,
                        UnitPrice = 4800m,
                        TotalAmount = 38400m,
                        DepartmentId = salesDepartment.Id
                    },
                    new SalesData
                    {
                        OrderNumber = "SLS-1003",
                        SaleDate = today.AddDays(-5),
                        CustomerName = "Nova Lojistik",
                        ProductName = "Danışmanlık Hizmeti",
                        Category = "Hizmet",
                        Quantity = 2,
                        UnitPrice = 18000m,
                        TotalAmount = 36000m,
                        DepartmentId = salesDepartment.Id
                    },
                    new SalesData
                    {
                        OrderNumber = "SLS-1004",
                        SaleDate = today.AddDays(-7),
                        CustomerName = "Eksen Holding",
                        ProductName = "Raporlama Eklentisi",
                        Category = "Yazılım",
                        Quantity = 12,
                        UnitPrice = 2750m,
                        TotalAmount = 33000m,
                        DepartmentId = salesDepartment.Id
                    },
                    new SalesData
                    {
                        OrderNumber = "SLS-1005",
                        SaleDate = today.AddDays(-10),
                        CustomerName = "Güneş Enerji",
                        ProductName = "Eğitim Paketi",
                        Category = "Hizmet",
                        Quantity = 4,
                        UnitPrice = 6500m,
                        TotalAmount = 26000m,
                        DepartmentId = salesDepartment.Id
                    },
                    new SalesData
                    {
                        OrderNumber = "SLS-1006",
                        SaleDate = today.AddDays(-14),
                        CustomerName = "Delta Yapı",
                        ProductName = "Kurumsal Lisans Paketi",
                        Category = "Yazılım",
                        Quantity = 3,
                        UnitPrice = 12500m,
                        TotalAmount = 37500m,
                        DepartmentId = salesDepartment.Id
                    }
                );
                await context.SaveChangesAsync();

            }
            #endregion
            #region Raporlanabilir tablo ve kolonları oluşturur
            if (!await context.ReportableTables.AnyAsync(x =>
        x.SchemaName == "dbo" &&
        x.TableName == "SalesData"))
            {
                var salesDepartment = await context.Departments
                    .SingleAsync(x => x.Name == "Satış");

                var reportableTable = new ReportableTable
                {
                    SchemaName = "dbo",
                    TableName = "SalesData",
                    DisplayName = "Satış Verileri",
                    DepartmentId = salesDepartment.Id,
                    IsActive = true
                };

                context.ReportableTables.Add(reportableTable);

                await context.SaveChangesAsync();

                context.ReportableColumns.AddRange(
                    new ReportableColumn
                    {
                        ReportableTableId = reportableTable.Id,
                        ColumnName = "OrderNumber",
                        DisplayName = "Sipariş No",
                        DataType = "string",
                        DisplayOrder = 1
                    },
                    new ReportableColumn
                    {
                        ReportableTableId = reportableTable.Id,
                        ColumnName = "SaleDate",
                        DisplayName = "Satış Tarihi",
                        DataType = "DateTime",
                        DisplayOrder = 2
                    },
                    new ReportableColumn
                    {
                        ReportableTableId = reportableTable.Id,
                        ColumnName = "CustomerName",
                        DisplayName = "Müşteri",
                        DataType = "string",
                        DisplayOrder = 3
                    },
                    new ReportableColumn
                    {
                        ReportableTableId = reportableTable.Id,
                        ColumnName = "ProductName",
                        DisplayName = "Ürün",
                        DataType = "string",
                        DisplayOrder = 4
                    },
                    new ReportableColumn
                    {
                        ReportableTableId = reportableTable.Id,
                        ColumnName = "Category",
                        DisplayName = "Kategori",
                        DataType = "string",
                        DisplayOrder = 5
                    },
                    new ReportableColumn
                    {
                        ReportableTableId = reportableTable.Id,
                        ColumnName = "Quantity",
                        DisplayName = "Adet",
                        DataType = "int",
                        DisplayOrder = 6
                    },
                    new ReportableColumn
                    {
                        ReportableTableId = reportableTable.Id,
                        ColumnName = "UnitPrice",
                        DisplayName = "Birim Fiyat",
                        DataType = "decimal",
                        DisplayOrder = 7
                    },
                    new ReportableColumn
                    {
                        ReportableTableId = reportableTable.Id,
                        ColumnName = "TotalAmount",
                        DisplayName = "Toplam Tutar",
                        DataType = "decimal",
                        DisplayOrder = 8
                    }
                );

                await context.SaveChangesAsync();
            }
            #endregion

        }
    }
}

