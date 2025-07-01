using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Nfe.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NfeDbContext>
{
    public NfeDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NfeDbContext>();
        var connectionString = "Host=localhost;Database=nfe_db;Username=postgres;Password=postgres;Port=5433";
        optionsBuilder.UseNpgsql(connectionString);
        return new NfeDbContext(optionsBuilder.Options);
    }
}