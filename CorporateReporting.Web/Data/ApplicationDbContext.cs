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
        }
    }
}
