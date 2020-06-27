using System.IO;
using CPK.Sso.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CPK.Sso.Factories
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var an = typeof(Startup).Assembly?.GetName()?.Name;
            optionsBuilder.UseNpgsql(config["ConnectionString"], npgsqlOptionsAction: o => o.MigrationsAssembly(an));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
