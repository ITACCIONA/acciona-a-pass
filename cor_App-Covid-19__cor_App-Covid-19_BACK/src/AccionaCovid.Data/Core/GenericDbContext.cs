using AccionaCovid.Data.Core;
using AccionaCovid.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccionaCovid.Data
{
    /// <summary>
    /// Clase que representa el contexto básico.
    /// </summary>
    public abstract class GenericDbContext : DbContext, IUnitOfWork
    {
        /// <summary>
        /// Transaccion en curso si la hay.
        /// </summary>
        protected IDbContextTransaction transaction;

        public GenericDbContext() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">Las opciones del DbContext.</param>
        public GenericDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Guarda los cambios de las entidades en la base de datos.
        /// </summary>
        public virtual new void SaveChanges()
        {
            SaveChanges(true);
        }

        /// <summary>
        /// Guarda los cambios de las entidades en la base de datos.
        /// </summary>
        /// <param name="audit"></param>
        public virtual new void SaveChanges(bool audit)
        {
            if(audit)
            {
                var auditEntries = OnBeforeSaveChanges();
                base.SaveChanges();
                OnAfterSaveChanges(auditEntries);
            }
            else
            {
                base.SaveChanges();
            }
        }

        /// <summary>
        /// Guarda los cambios de las entidades en la base de datos asíncronamente.
        /// </summary>
        public virtual async Task SaveChangesAsync()
        {
            await SaveChangesAsync(true).ConfigureAwait(false);
        }

        /// <summary>
        /// Guarda los cambios de las entidades en la base de datos asíncronamente.
        /// </summary>
        public virtual async Task SaveChangesAsync(bool audit)
        {
            if (audit)
            {
                var auditEntries = OnBeforeSaveChanges();
                await base.SaveChangesAsync().ConfigureAwait(false);
                await OnAfterSaveChanges(auditEntries);
            }
            else
            {
                await base.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Metodo que inicia una transaccion.
        /// </summary>
        public void ExecuteQuery(string query)
        {
            Database.ExecuteSqlCommand(query);
        }

        /// <summary>
        /// Metodo que inicia una transaccion.
        /// </summary>
        public void BeginTransaction()
        {
            transaction = Database.BeginTransaction();
        }

        /// <summary>
        /// Commit de la transaccion.
        /// </summary>
        public void Commit()
        {
            try
            {
                if (transaction != null)
                {
                    transaction.Commit();
                    transaction.Dispose();
                    transaction = null;
                }
            }
            catch
            {
                if (transaction != null)
                    transaction.Rollback();

                throw;
            }
        }

        /// <summary>
        /// Rollback de la transacción.
        /// </summary>
        public void Rollback()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
                transaction = null;
            }
        }

        internal abstract List<AuditEntry> OnBeforeSaveChanges();

        internal abstract Task OnAfterSaveChanges(List<AuditEntry> auditEntries);
    }
}
