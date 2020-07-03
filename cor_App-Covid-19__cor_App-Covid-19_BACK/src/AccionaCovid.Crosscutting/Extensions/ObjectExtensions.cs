using System;

namespace AccionaCovid.Crosscutting
{
    /// <summary>
    /// Extensiones para el tipo Object
    /// </summary>
    public static class ObjectExtensions
    {
        public static T Modify<T>(this T t, Action<T> modify) where T : class
        {
            modify(t);
            return t;
        }

        /// <summary>
        /// Si un objeto es null, lanza una excepción ArgumentNullException
        /// </summary>
        /// <param name="obj">El objeto a comprobar</param>
        /// <param name="name">El nombre a indicar en la excepción</param>
        public static void ThrowIfNull(this object obj, string name)
        {
            if (obj == null) throw new ArgumentNullException(name);
        }

        /// <summary>
        /// Si un objeto es null, dvuelve ifNull
        /// </summary>
        /// <param name="obj">El objeto a comprobar</param>
        /// <param name="ifNull">El valor a devolver si el objeto es null</param>
        public static T Coalesce<T>(this T obj, T ifNull) where T : class
        {
            return obj ?? ifNull;
        }
    }
}
