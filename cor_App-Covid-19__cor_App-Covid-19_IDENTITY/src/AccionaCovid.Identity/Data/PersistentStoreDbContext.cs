using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Identity.Data
{
    public class PersistentStoreDbContext: PersistedGrantDbContext<PersistentStoreDbContext>
    {
        public PersistentStoreDbContext(DbContextOptions<PersistentStoreDbContext> options, OperationalStoreOptions storeOptions) : base(options, storeOptions)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);
    }
}
