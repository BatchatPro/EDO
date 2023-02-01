using EDO.Database.Models;
using Microsoft.EntityFrameworkCore;
namespace EDO.Database;

public class EdoDbContext:DbContext
{
    public DbSet<Document> Documents { get; set; }

    public EdoDbContext()
    {
        Database.Migrate();
    }

    public EdoDbContext(DbContextOptions<EdoDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

}
