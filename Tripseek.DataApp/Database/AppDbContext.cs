using Tripseek.DataApp.DTOs.InternalApi;

namespace Tripseek.DataApp.Database
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server='tripseekdb.mysql.database.azure.com';UserID = 'tripseek';Password='TS2022HackKosice';Database='tripseekdata';SslMode=Required;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.Create(new Version("8.0.0"), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql), options =>
            {
                options.EnableRetryOnFailure();
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
