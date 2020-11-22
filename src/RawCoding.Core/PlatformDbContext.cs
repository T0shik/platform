using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RawCoding.Data
{
    public class PlatformDbContext :
        IdentityDbContext<PlatformUser>,
        IDataProtectionKeyContext
    {
        public PlatformDbContext(DbContextOptions<PlatformDbContext> options) : base(options)
        {
        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    }
}