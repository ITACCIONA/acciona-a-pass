using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Crosscutting
{
    /// <summary>
    /// Clase de extensión para Queryable.
    /// </summary>
    public static class IQueryableExtensions
    {
        ///// <summary>
        ///// Quitar el tracking.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="query"></param>
        ///// <returns></returns>
        //public static IQueryable<T> AsNoTracking<T>(this IQueryable<T> query) where T : class
        //{
        //    return EntityFrameworkQueryableExtensions.AsNoTracking(query);
        //}

        /// <summary>
        /// FirstOrDefault asíncrono.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> query) where T : class
        {
            return EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query);
        }

        /// <summary>
        /// SingleOrDefault asíncrono.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static Task<T> SingleOrDefaultAsync<T>(this IQueryable<T> query) where T : class
        {
            return EntityFrameworkQueryableExtensions.SingleOrDefaultAsync(query);
        }

        /// <summary>
        /// Count asíncrono.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static Task<int> CountAsync<T>(this IQueryable<T> query) where T : class
        {
            return EntityFrameworkQueryableExtensions.CountAsync(query);
        }

        ///// <summary>
        ///// ToDictionary asíncrono.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="TKey"></typeparam>
        ///// <typeparam name="TElement"></typeparam>
        ///// <param name="source"></param>
        ///// <param name="keySelector"></param>
        ///// <param name="elementSelector"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<T, TKey, TElement>(this IQueryable<T> source, Func<T, TKey> keySelector,
        //    Func<T, TElement> elementSelector, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        //{
        //    return EntityFrameworkQueryableExtensions.ToDictionaryAsync(source, keySelector, elementSelector, cancellationToken);
        //}

        /// <summary>
        /// ToListAsync asíncrono.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> query) where T : class
        {
            return await EntityFrameworkQueryableExtensions.ToListAsync(query).ConfigureAwait(false);
        }

        /// <summary>
        /// ToList asíncrono.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static async Task<List<TResult>> ToListAsync<T, TResult>(this IQueryable<T> query, Func<T, TResult> selector) where T : class where TResult : class
        {
            List<T> list = await EntityFrameworkQueryableExtensions.ToListAsync(query).ConfigureAwait(false);
            return list.Select(selector).ToList();
        }

        ///// <summary>
        ///// Include.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="TProperty"></typeparam>
        ///// <param name="source"></param>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> source, Expression<Func<T, TProperty>> path) where T : class
        //{
        //    return EntityFrameworkQueryableExtensions.Include(source, path);
        //}
    }
}
