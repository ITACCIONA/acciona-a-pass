using System;
using System.Collections.Generic;
using System.Linq;

namespace AccionaCovid.Crosscutting
{
    /// <summary>
    /// Clase extension de enumerables
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// Genera un rango de fechas para iterar usando el periodo indicado.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> Range(DateTime start, DateTime end, TimeSpan? step = null)
        {
            step = step ?? TimeSpan.FromDays(1);

            while (start < end)
            {
                yield return start;
                start = start.Add(step.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source) action(item);
        }

        /// <summary>
        /// Determina si algún elemento se encuentra en la colección
        /// </summary>
        /// <typeparam name="T">Tipo de los elementos de la colección</typeparam>
        /// <param name="source">Colección inicial</param>
        /// <param name="elements">Elementos a buscar en la colección</param>
        /// <returns>Indica si algún elemento se encuentra en la colección</returns>
        public static bool ContainsAny<T>(this IEnumerable<T> source, params T[] elements)
        {
            return source.Intersect(elements.ToList()).Any();
        }

        /// <summary>
        /// Determina si todos los elementos se encuentran en la colección y no hay ninguno más
        /// </summary>
        /// <typeparam name="T">Tipo de los elementos de la colección</typeparam>
        /// <param name="source">Colección inicial</param>
        /// <param name="elements">Elementos a buscar en la colección</param>
        /// <returns>Indica si todos los elemento se encuentran en la y no hay ninguno más</returns>
        public static bool ContainsAllExact<T>(this IEnumerable<T> source, params T[] elements)
        {
            return !source.Except(elements.ToList()).Any();
        }

        /// <summary>
        /// If source is in elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static bool IsAny<T>(this T source, params T[] elements)
        {
            return elements.Contains(source);
        }
    }
}
