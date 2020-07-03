using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccionaCovid.Domain.Core
{
    /// <summary>
    /// Interfaz que representa las operaciones mínimas de una unidad de trabajo
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        ///<remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,  
        /// then an exception is thrown
        ///</remarks>
        void SaveChanges();

        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        ///<remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,  
        /// then an exception is thrown
        ///</remarks>
        void SaveChanges(bool audit);

        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        ///<remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,  
        /// then an exception is thrown
        ///</remarks>
        Task SaveChangesAsync();

        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        ///<remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,  
        /// then an exception is thrown
        ///</remarks>
        Task SaveChangesAsync(bool audit);

        /// <summary>
        /// Metodo que arranca una transaccion
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commit de la transaccion
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback de la transaccion
        /// </summary>
        void Rollback();

    }
}
