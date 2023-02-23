using EDO.Access.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EDO.Access
{
    public class AccessDbContext:IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public AccessDbContext(DbContextOptions<AccessDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            Database.Migrate();
        }
    }
}