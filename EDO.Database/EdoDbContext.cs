using EDO.Database.Models;
using EDO.Database.Models.AccessReferences;
using EDO.Domain;
using Microsoft.EntityFrameworkCore;

namespace EDO.Database;

public class EdoDbContext:BoundedDbContext<EdoDbContext>
{

    public EdoDbContext(DbContextOptions<EdoDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        Database.Migrate();
    }
    public DbSet<Document> Documents { get; set; }
    public DbSet<DocumentType> DocumentTypes { get; set; }
    public DbSet<DocumentUser> DocumentUsers { get; set; }
    public DbSet<ApplicationUserReference> ApplicationUserReferences { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Document>().HasOne(x => x.DocumentType).WithMany(x => x.Documents);
        base.OnModelCreating(builder);

        builder.Entity<DocumentUser>()
        .HasKey(bc => new { bc.UserId, bc.DocumentId });
        builder.Entity<DocumentUser>()
            .HasOne(bc => bc.User)
            .WithMany(b => b.DocumentUsers)
            .HasForeignKey(bc => bc.UserId);
        builder.Entity<DocumentUser>()
            .HasOne(bc => bc.Document)
            .WithMany(c => c.DocumentUsers)
            .HasForeignKey(bc => bc.DocumentId);
        builder.Entity<ApplicationUserReference>()
            .ToTable("AspNetUsers", t => t.ExcludeFromMigrations());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

}
