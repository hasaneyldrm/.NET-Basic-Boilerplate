// AppDbContext.cs
using BasicBoilerplate.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace BasicBoilerplate
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("MySQLConnection");
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
               .Property(u => u.Id)
                 .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasColumnName("createdAt");

            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasColumnName("updatedAt");
        }
    }
}
