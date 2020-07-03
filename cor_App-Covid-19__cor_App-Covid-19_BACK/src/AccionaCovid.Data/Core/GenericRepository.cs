using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EFCore.BulkExtensions;

namespace AccionaCovid.Data
{
    /// <summary>
    /// Clase que implementa las operaciones mínimas de un repositorio.
    /// </summary>
    /// <typeparam name="T">El tipo de la entidad.</typeparam>
    public class GenericRepository<T> : IRepository<T> where T : Entity<T>
    {
        /// <summary>
        /// Inidca si se aplica la logica de eliminado logico
        /// </summary>
        bool logicalRemove;

        /// <summary>
        /// Usuario que realiza la peticion
        /// </summary>
        private readonly IUserInfoAccesor userInfoAccesor;

        /// <summary>
        /// Contexto de base de datos concreto a usar.
        /// </summary>
        public AccionaCovidContext Context { get; set; }

        /// <summary>
        /// Unidad de trabajo para sincronizar operaciones mediante una transacción.
        /// </summary>
        public IUnitOfWork UnitOfWork => Context;

        /// <summary>
        /// Constructor con la inyección del contexto.
        /// </summary>
        /// <param name="context"></param>
        public GenericRepository(AccionaCovidContext context, IUserInfoAccesor userInfoAccesor, bool logicalRemove)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.logicalRemove = logicalRemove;
            this.userInfoAccesor = userInfoAccesor ?? throw new ArgumentNullException(nameof(userInfoAccesor));
        }

        /// <summary>
        /// Metodo que reasigna un nuevo contexto al repositorio. Se usa para compartir un mismo contexto entre diferentes repositorios
        /// para usar transacciones.
        /// </summary>
        /// <param name="newContext">Nuevo contexto a usar en el repositorio.</param>
        public void AssignNewContext(IUnitOfWork newContext)
        {
            //Context.Dispose();
            Context = null;

            if (newContext is AccionaCovidContext)
            {
                Context = newContext as AccionaCovidContext;
            }
        }

        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        ///<remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,  
        /// then an exception is thrown.
        ///</remarks>
        public void SaveChanges()
        {
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        ///<remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,  
        /// then an exception is thrown.
        ///</remarks>
        public async Task SaveChangesAsync()
        {
            await UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Attach.
        /// </summary>
        /// <param name="entity"></param>
        public void Attach(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
                Context.Attach(entity);
        }

        /// <summary>
        /// Añade una entidad.
        /// </summary>
        /// <param name="entity">La entidad a añadir.</param>
        public void Add(T entity)
        {
            if (logicalRemove)
            {
                entity.Deleted = false;
            }

            Context.Add(entity);
        }

        /// <summary>
        /// Añade o actualiza una entidad.
        /// </summary>
        /// <param name="entity">La entidad a añadir</param>
        public void AddOrUpdate(T entity) => Context.Attach(entity);

        /// <summary>
        /// Elimina una entidad.
        /// </summary>
        /// <param name="entity">La entidad a añadir.</param>
        public void Remove(T entity)
        {
            if (logicalRemove)
            {
                entity.Deleted = true;
                Context.Update(entity);
            }
            else
                Context.Remove(entity);
        }

        /// <summary>
        /// Actualiza una entidad.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        public void Update(T entity)
        {
            if (logicalRemove)
            {
                entity.Deleted = false;
            }

            Context.Update(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="newValues"></param>
        public void UpdateWith(T entity, object newValues)
        {
            EntityEntry<T> entry = Context.Entry(entity);
            entry.CurrentValues.SetValues(newValues);
            entry.State = EntityState.Modified;
        }

        /// <summary>
        /// Añade una entidad.
        /// </summary>
        /// <param name="entities">Las entidades a añadir</param>
        public void AddRange(IEnumerable<T> entities)
        {
            if (logicalRemove)
            {
                foreach (T entity in entities)
                {
                    entity.Deleted = false;
                    entity.LastAction = "CREATE";
                    entity.LastActionDate = DateTime.UtcNow;
                    // las propiedades IdUser y UserName se realiza desde la clase contexto (OnBeforeSaveChanges) la cual es llamada desde el saveChanges
                    //entity.IdUser = userInfoAccesor.IdUser;
                    //entity.UserName = userInfoAccesor.UserFullName;
                }
            }

            Context.AddRange(entities);
        }

        /// <summary>
        /// Elimina una entidad.
        /// </summary>
        /// <param name="entities">Las entidades a borrar</param>
        public void RemoveRange(IEnumerable<T> entities)
        {
            if (logicalRemove)
            {
                foreach (T entity in entities)
                {
                    entity.Deleted = true;
                    entity.LastAction = "DELETE";
                    entity.LastActionDate = DateTime.UtcNow;
                    // las propiedades IdUser y UserName se realiza desde la clase contexto (OnBeforeSaveChanges) la cual es llamada desde el saveChanges
                    //entity.IdUser = userInfoAccesor.IdUser;
                    //entity.UserName = userInfoAccesor.UserFullName;
                }
                Context.UpdateRange(entities);
            }
            else
            {
                Context.RemoveRange(entities);
            }
        }

        /// <summary>
        /// Elimina una entidad.
        /// </summary>
        /// <param name="entities">Las entidades a borrar</param>
        public void DeletedRange(IEnumerable<T> entities)
        {
            Context.RemoveRange(entities);
        }


        /// <summary>
        /// Elimina todos los elementos de una tabla.
        /// </summary>
        public void RemoveAll()
        {
            Context.Set<T>().RemoveRange(Context.Set<T>());
        }

        /// <summary>
        /// Actualiza una entidad.
        /// </summary>
        /// <param name="entities">Las entidades a actualizar.</param>
        public void UpdateRange(IEnumerable<T> entities)
        {
            if (logicalRemove)
            {
                foreach (T entity in entities)
                {
                    entity.Deleted = false;
                    entity.LastAction = "UPDATE";
                    entity.LastActionDate = DateTime.UtcNow;
                    // las propiedades IdUser y UserName se realiza desde la clase contexto (OnBeforeSaveChanges) la cual es llamada desde el saveChanges
                    //entity.IdUser = userInfoAccesor.IdUser;
                    //entity.UserName = userInfoAccesor.UserFullName;
                }
            }

            Context.UpdateRange(entities);
        }

        /// <summary>
        /// Inserccion de forma masiva de una lista
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="update"></param>
        public async Task BulkInsertAsync(IList<T> entities)
        {
            var bulkConfig = new BulkConfig() { PreserveInsertOrder = true, SetOutputIdentity = true };
            //var bulkConfig = new BulkConfig() { SetOutputIdentity = true };

            await Context.BulkInsertAsync(entities, bulkConfig).ConfigureAwait(false);
        }

        /// <summary>
        /// Inserccion de forma masiva de una lista
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="update"></param>
        public async Task BulkInsertWithOutSetOutputAsync(IList<T> entities)
        {
            await Context.BulkInsertAsync(entities).ConfigureAwait(false);
        }

        /// <summary>
        /// Inserccion de forma masiva de una lista
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="update"></param>
        public async Task BulkUpdateAsync(IList<T> entities)
        {
            await Context.BulkUpdateAsync(entities).ConfigureAwait(false);
        }

        /// <summary>
        /// Carga los elementos hijos de un elemento.
        /// </summary>
        /// <param name="selector">El selector del elemento.</param>
        /// <returns>true si un elemento cumple la condición.</returns>
        public async Task LoadChildren(T entity, Expression<Func<T, IEnumerable<object>>> selector)
        {
            await Context.Entry(entity).Collection(selector)
                .LoadAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Carga los elementos hijos de un elemento.
        /// </summary>
        /// <param name="selector">El selector del elemento.</param>
        /// <returns>true si un elemento cumple la condición.</returns>
        public async Task LoadChil(T entity, Expression<Func<T, object>> selector)
        {
            await Context.Entry(entity).Reference(selector)
                .LoadAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Devuelve true si un elemento cumple la condición.
        /// </summary>
        /// <param name="predicate">La condición.</param>
        /// <returns>true si un elemento cumple la condición.</returns>
        public async Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            if (logicalRemove)
                return await Context.Set<T>().Where(c => !c.Deleted).AnyAsync(predicate).ConfigureAwait(false);
            else
                return await Context.Set<T>().AnyAsync(predicate).ConfigureAwait(false);
        }

        /// <summary>
        /// Devuelve true si un elemento hijo cumple la condición.
        /// </summary>
        /// <param name="selector">La condición.</param>
        /// <returns>true si un elemento cumple la condición.</returns>
        public async Task<bool> AnyChild<TChild>(T entity, Expression<Func<T, IEnumerable<TChild>>> selector) where TChild : class
        {
            return await Context.Entry(entity)
                .Collection(selector).Query()
                .AnyAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Devuelve true si un elemento hijo cumple la condición.
        /// </summary>
        /// <param name="predicate">La condición.</param>
        /// <returns>true si un elemento cumple la condición.</returns>
        public async Task<bool> AnyChild<TChild>(T entity, Expression<Func<T, IEnumerable<TChild>>> selector, Expression<Func<TChild, bool>> predicate) where TChild : class
        {
            return await Context.Entry(entity)
                .Collection(selector).Query()
                .AnyAsync(predicate).ConfigureAwait(false);
        }

        /// <summary>
        /// Obtiene todos los elementos que cumplen una condición.
        /// </summary>
        /// <returns>Los elementos que cumplen una condición.</returns>
        public IQueryable<T> FromSqlRaw(string sql)
        {
            return Context.Set<T>().FromSqlRaw(sql);
        }

        /// <summary>
        /// Obtiene un elemento por su id.
        /// </summary>
        /// <param name="id">El id a buscar.</param>
        /// <returns>El elemento o null si no existe.</returns>
        public async Task<T> GetById(int id)
        {
            if (logicalRemove)
                return await Context.Set<T>().Where(c => !c.Deleted).FirstOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);
            else
                return await Context.Set<T>().FindAsync(id).ConfigureAwait(false);
        }

        /// <summary>
        /// Obtiene todos los elementos que cumplen una condición.
        /// </summary>
        /// <returns>Los elementos que cumplen una condición.</returns>
        public IQueryable<T> GetAll()
        {
            if (logicalRemove)
                return Context.Set<T>().Where(c => !c.Deleted);
            else
                return Context.Set<T>();
        }

        /// <summary>
        /// Obtiene todos los elementos.
        /// </summary>
        /// <param name="includes">Las propiedades a incluir (solo el primer nivel de cada una).</param>
        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            if (logicalRemove)
                return Include(Context.Set<T>().Where(c => !c.Deleted), includes);
            else
                return Include(Context.Set<T>(), includes);
        }

        /// <summary>
        /// Obtiene todos los elementos.
        /// </summary>
        /// <param name="includes">Las propiedades a incluir (solo el primer nivel de cada una).</param>
        public IQueryable<T> GetAll(params string[] includes)
        {
            if (logicalRemove)
                return Include(Context.Set<T>().Where(c => !c.Deleted), includes);
            else
                return Include(Context.Set<T>(), includes);
        }

        /// <summary>
        /// Obtiene todos los elementos.
        /// </summary>
        /// <returns>Todos los elementos</returns>
        public IQueryable<T> GetBy(Expression<Func<T, bool>> predicate)
        {
            if (logicalRemove)
            {
                predicate = predicate.And(c => !c.Deleted);
            }
            return Context.Set<T>().Where(predicate);
        }

        /// <summary>
        /// Obtiene todos los elementos que cumplen una condición.
        /// </summary>
        /// <param name="predicate">La condición.</param>
        /// <param name="includes">Las propiedades a incluir (solo el primer nivel de cada una).</param>
        /// <returns>Los elementos que cumplen una condición.</returns>
        public IQueryable<T> GetBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            if (logicalRemove)
            {
                predicate = predicate.And(c => !c.Deleted);
            }
            return Include(Context.Set<T>(), includes).Where(predicate);
        }

        /// <summary>
        /// Obtiene todos los elementos que cumplen una condición.
        /// </summary>
        /// <param name="predicate">La condición.</param>
        /// <param name="includes">Las propiedades a incluir (solo el primer nivel de cada una).</param>
        /// <returns>Los elementos que cumplen una condición.</returns>
        public IQueryable<T> GetBy(Expression<Func<T, bool>> predicate, params string[] includes)
        {
            if (logicalRemove)
            {
                predicate = predicate.And(c => !c.Deleted);
            }
            return Include(Context.Set<T>(), includes).Where(predicate);
        }

        /// <summary>
        /// Añade los includes indicados a una query.
        /// </summary>
        /// <param name="query">La query a la que se añaden los includes.</param>
        /// <param name="includes">Los includes a añadir (solo primer nivel).</param>
        /// <returns>Query con los includes ya añadidos</returns>
        protected IQueryable<T> Include(IQueryable<T> query, Expression<Func<T, object>>[] includes)
        {
            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return query;
        }

        /// <summary>
        /// Añade los includes indicados a una query.
        /// </summary>
        /// <param name="query">La query a la que se añaden los includes.</param>
        /// <param name="includes">Los includes a añadir.</param>
        /// <returns>Query con los includes ya añadidos.</returns>
        protected IQueryable<T> Include(IQueryable<T> query, string[] includes)
        {
            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return query;
        }
    }
}
