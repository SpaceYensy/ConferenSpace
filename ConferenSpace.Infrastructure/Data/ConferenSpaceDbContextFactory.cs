using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ConferenSpace.Infrastructure.Data;

public class ConferenSpaceDbContextFactory : IDesignTimeDbContextFactory<ConferenSpaceDbContext>
{
    public ConferenSpaceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ConferenSpaceDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ConferenSpaceDb;Trusted_Connection=true;");
        
        return new ConferenSpaceDbContext(optionsBuilder.Options);
    }
}
