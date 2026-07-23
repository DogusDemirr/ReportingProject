using CorporateReporting.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporateReporting.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Users DbSet for managing application users in the database.
        /// </summary>
        public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
        /// <summary>
        /// Roles DbSet for managing application roles in the database.
        /// </summary>
        public DbSet<Role> Roles => Set<Role>();
        /// <summary>
        /// Departments DbSet for managing departments in the database.
        /// </summary>
        public DbSet<Department> Departments => Set<Department>();
        /// <summary>
        /// ReportableTables
        /// </summary>
        public DbSet<ReportableTable> ReportableTables => Set<ReportableTable>();
        /// <summary>
        /// ReportableColumns
        /// </summary>
        public DbSet<ReportableColumn> ReportableColumns => Set<ReportableColumn>();
        /// <summary>
        /// SalesData
        /// </summary>
        public DbSet<SalesData> SalesData => Set<SalesData>();
        /// <summary>
        /// ReportTemplates
        /// </summary>
        public DbSet<ReportTemplate> ReportTemplates => Set<ReportTemplate>();
        /// <summary>
        /// AuditLogs DbSet for managing audit logs in the database.
        /// </summary>
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");

                entity.HasIndex(x => x.Email).IsUnique();

                entity.Property(x => x.FullName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.Email)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.PasswordHash)
                    .IsRequired();

                entity.HasOne(x => x.Role)
                    .WithMany()
                    .HasForeignKey(x => x.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Department)
                    .WithMany()
                    .HasForeignKey(x => x.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(x => x.Name)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.HasIndex(x => x.Name).IsUnique();
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(x => x.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasIndex(x => x.Name).IsUnique();
            });
            modelBuilder.Entity<ReportableTable>(entity =>
            {
                entity.Property(x => x.SchemaName)
                    .HasMaxLength(128)
                    .IsRequired();

                entity.Property(x => x.TableName)
                    .HasMaxLength(128)
                    .IsRequired();

                entity.Property(x => x.DisplayName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.HasIndex(x => new { x.SchemaName, x.TableName })
                    .IsUnique();

                entity.HasOne(x => x.Department)
                    .WithMany()
                    .HasForeignKey(x => x.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<ReportableColumn>(entity =>
            {
                entity.Property(x => x.ColumnName)
                    .HasMaxLength(128)
                    .IsRequired();

                entity.Property(x => x.DisplayName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.DataType)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.HasIndex(x => new { x.ReportableTableId, x.ColumnName })
                    .IsUnique();

                entity.HasOne(x => x.ReportableTable)
                    .WithMany()
                    .HasForeignKey(x => x.ReportableTableId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SalesData>(entity =>
            {
                entity.Property(x => x.OrderNumber)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(x => x.CustomerName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.ProductName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.Category)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.UnitPrice)
                    .HasPrecision(18, 2);

                entity.Property(x => x.TotalAmount)
                    .HasPrecision(18, 2);

                entity.HasIndex(x => x.OrderNumber);

                entity.HasIndex(x => x.SaleDate);

                entity.HasOne(x => x.Department)
                    .WithMany()
                    .HasForeignKey(x => x.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ReportTemplate>(entity =>
            {
                entity.Property(x => x.Name)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.ConfigurationJson)
                    .IsRequired();

                entity.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.ReportableTable)
                    .WithMany()
                    .HasForeignKey(x => x.ReportableTableId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(x => new { x.UserId, x.Name })
                    .IsUnique();
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.Property(x => x.Action)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.Details)
                    .HasMaxLength(2000);

                entity.Property(x => x.IpAddress)
                    .HasMaxLength(64);

                entity.HasIndex(x => x.CreatedAt);

                entity.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
