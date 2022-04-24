using Tripseek.DataApp.DTOs.InternalApi;

namespace Tripseek.DataApp.Database
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationManager.Initialize();
            var connectionString = ConfigurationManager.Configuration.ConnectionString;
            optionsBuilder.UseMySql(connectionString, ServerVersion.Create(new Version("8.0.0"), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql), options =>
            {
                options.EnableRetryOnFailure();
            }).EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
