using ClassLibrary2.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClassLibrary1
{
    public class Contexto : DbContext
    {
        private IConfiguration _configuration;

        public DbSet<TechnicalVisits> TechnicalVisits { get; set; }

        public Contexto(IConfiguration configuration, DbContextOptions options) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var typeDatabase = _configuration["TypeDatabase"];
            var connectionString = _configuration.GetConnectionString(typeDatabase);

            if (typeDatabase == "SqlServer") 
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
            else if (typeDatabase == "Postgresql")
            {
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}