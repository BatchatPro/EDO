using EDO.Database.Models;
using Microsoft.EntityFrameworkCore;
namespace EDO.Database;

public class EdoDbContext:DbContext
{
    public EdoDbContext()
    {
        Database.Migrate();
    }

    public EdoDbContext(DbContextOptions<EdoDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        Database.Migrate();
    }
    public DbSet<Document> Documents { get; set; }
    public DbSet<DocumentType> DocumentTypes { get; set; }
    //public DbSet<DocumentUser> DocumentUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Document>().HasOne(x => x.DocumentType).WithMany(x => x.Documents);
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

}
