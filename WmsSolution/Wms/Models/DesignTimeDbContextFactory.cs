using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Wms.Models;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDataContext>
{
    public AppDataContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDataContext>()
            // Use a MESMA connection string do appsettings.json
            .UseSqlite("Data Source=Wms.db")
            .Options;

        return new AppDataContext(options);
    }
}