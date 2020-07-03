using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AccionaCovid.Domain.Core
{
    /// <summary>
    /// Interfaz que representa las operaciones mínimas de un repositorio
    /// </summary>
    /// <typeparam name="T">El tipo de la entidad</typeparam>
    public interface IRepository<T> where T : Entity<T>
    {
        /// <summary>
        /// Unidad de trabajo para sincronizar operaciones mediante una transacción.
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Metodo que reasigna un nuevo contexto al repositorio. Se usa para compartir un mismo contexto entre diferentes repositorios
        /// para usar transacciones.
        /// </summary>
        /// <param name="newContext">Nuevo contexto a usar en el repositorio.</param>
        void AssignNewContext(IUnitOfWork newContext);

        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        ///<remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,  
        /// then an exception is thrown.
        ///</remarks>
        void SaveChanges();

        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        ///<remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,  
        /// then an exception is thrown.
        ///</remarks>
        Task SaveChangesAsync();

        /// <summary>
        /// Añade una entidad.
        /// </summary>
        /// <param name="entity">La entidad a añadir.</param>
        void Add(T entity);

        /// <summary>
        /// Añade o actualiza una entidad.
        /// </summary>
        /// <param name="entity">La entidad a añadir</param>
        void AddOrUpdate(T entity);

        /// <summary>
        /// Elimina una entidad.
        /// </summary>
        /// <param name="entity">La entidad a añadir.</param>
        void Remove(T entity);

        /// <summary>
        /// Añade una entidad.
        /// </summary>
        /// <param name="entity">La entidad a añadir.</param>
        void Attach(T entity);

        /// <summary>
        /// Actualiza una entidad.
        /// </summary>
        /// <param name="entity">La entidad a añadir.</param>
        void Update(T entity);

        /// <summary>
        /// Actualiza una entidad.
        /// </summary>
        /// <param name="entity">La entidad a añadir.</param>
        void UpdateWith(T entity, object newValues);

        ///// <summary>
        ///// Actualiza una entidad.
        ///// </summary>
        ///// <param name="entity">La entidad a añadir.</param>
        //void UpdateCollection<TChild>(T entity, T newValues, Func<T, ICollection<TChild>> selector) where TChild : Entity<TChild>;

        ///// <summary>
        ///// Actualiza una entidad.
        ///// </summary>
        ///// <param name="entity">La entidad a añadir.</param>
        //void UpdateRecursiveCollection<TFirst, TSecond>(T entity, T newValues, Func<T, ICollection<TFirst>> selector, Func<TFirst, ICollection<TSecond>> childSelector) where TFirst : Entity<TFirst> where TSecond : Entity<TSecond>;

        /// <summary>
        /// Añade una entidad.
        /// </summary>
        /// <param name="entities">Las entidades a añadir.</param>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Elimina una entidad.
        /// </summary>
        /// <param name="entities">Las entidades a borrar.</param>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Elimina una entidad.
        /// </summary>
        /// <param name="entities">Las entidades a borrar.</param>
        void DeletedRange(IEnumerable<T> entities);

        /// <summary>
        /// Elimina una entidad.
        /// </summary>
        /// <param name="entities">Las entidades a borrar.</param>
        void RemoveAll();

        /// <summary>
        /// Actualiza una entidad.
        /// </summary>
        /// <param name="entities">Las entidades a actualizar.</param>
        void UpdateRange(IEnumerable<T> entities);

        /// <summary>
        /// Inserccion de forma masiva de una lista
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="update"></param>
        Task BulkInsertAsync(IList<T> entities);

        /// <summary>
        /// Inserccion de forma masiva de una lista
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="update"></param>
        Task BulkInsertWithOutSetOutputAsync(IList<T> entities);

        /// <summary>
        /// Actualizacion de forma masiva de una lista
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="update"></param>
        Task BulkUpdateAsync(IList<T> entities);

        /// <summary>
        /// Carga los elementos hijos de un elemento.
        /// </summary>
        /// <param name="selector">El selector del elemento.</param>
        /// <returns>true si un elemento cumple la condición.</returns>
        Task LoadChildren(T entity, Expression<Func<T, IEnumerable<object>>> selector);

        /// <summary>
        /// Carga los elementos hijos de un elemento.
        /// </summary>
        /// <param name="selector">El selector del elemento.</param>
        /// <returns>true si un elemento cumple la condición.</returns>
        Task LoadChil(T entity, Expression<Func<T, object>> selector);

        /// <summary>
        /// Devuelve true si un elemento cumple la condición.
        /// </summary>
        /// <param name="predicate">La condición.</param>
        /// <returns>true si un elemento cumple la condición.</returns>
        Task<bool> Any(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Devuelve true si un elemento hijo cumple la condición.
        /// </summary>
        /// <param name="selector">La condición.</param>
        /// <returns>true si un elemento cumple la condición.</returns>
        Task<bool> AnyChild<TChild>(T entity, Expression<Func<T, IEnumerable<TChild>>> selector) where TChild : class;

        /// <summary>
        /// Devuelve true si un elemento hijo cumple la condición.
        /// </summary>
        /// <param name="predicate">La condición.</param>
        /// <returns>true si un elemento cumple la condición.</returns>
        Task<bool> AnyChild<TChild>(T entity, Expression<Func<T, IEnumerable<TChild>>> selector, Expression<Func<TChild, bool>> predicate) where TChild : class;

        /// <summary>
        /// Obtiene todos los elementos que cumplen una condición.
        /// </summary>
        /// <returns>Los elementos que cumplen una condición.</returns>
        IQueryable<T> FromSqlRaw(string sql);

        /// <summary>
        /// Obtiene un elemento por su id.
        /// </summary>
        /// <param name="id">El id a buscar.</param>
        /// <returns>El elemento o null si no existe.</returns>
        Task<T> GetById(int id);

        /// <summary>
        /// Obtiene todos los elementos.
        /// </summary>
        /// <returns>Todos los elementos.</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Obtiene todos los elementos que cumplen una condicion.
        /// </summary>
        /// <param name="predicate">La condición.</param>
        /// <returns>Los elementos que cumplen una condición.</returns>
        IQueryable<T> GetBy(Expression<Func<T, bool>> predicate);

        ///// <summary>
        ///// Obtiene un elemento por su id.
        ///// </summary>
        ///// <param name="id">El id a buscar.</param>
        ///// <param name="includes">Las propiedades a incluir (solo el primer nivel de cada una).</param>
        ///// <returns>El elemento o null si no existe.</returns>
        //Task<T> GetById(int id, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Obtiene todos los elementos.
        /// </summary>
        /// <param name="includes">Las propiedades a incluir (solo el primer nivel de cada una).</param>
        /// <returns>Todos los elementos.</returns>
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Obtiene todos los elementos que cumplen una condicion.
        /// </summary>
        /// <param name="predicate">La condición.</param>
        /// <param name="includes">Las propiedades a incluir (solo el primer nivel de cada una).</param>
        /// <returns>Los elementos que cumplen una condición.</returns>
        IQueryable<T> GetBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        ///// <summary>
        ///// Obtiene un elemento por su id.
        ///// </summary>
        ///// <param name="id">El id a buscar.</param>
        ///// <param name="includes">Las propiedades a incluir (solo el primer nivel de cada una).</param>
        ///// <returns>El elemento o null si no existe.</returns>
        //Task<T> GetById(int id, params string[] includes);

        /// <summary>
        /// Obtiene todos los elementos.
        /// </summary>
        /// <param name="includes">Las propiedades a incluir (solo el primer nivel de cada una).</param>
        /// <returns>Todos los elementos.</returns>
        IQueryable<T> GetAll(params string[] includes);

        /// <summary>
        /// Obtiene todos los elementos que cumplen una condición.
        /// </summary>
        /// <param name="predicate">La condición.</param>
        /// <param name="includes">Las propiedades a incluir (solo el primer nivel de cada una).</param>
        /// <returns>Los elementos que cumplen una condición.</returns>
        IQueryable<T> GetBy(Expression<Func<T, bool>> predicate, params string[] includes);
    }
}
