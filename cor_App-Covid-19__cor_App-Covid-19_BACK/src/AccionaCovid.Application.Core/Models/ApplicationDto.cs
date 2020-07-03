using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.Application.Core
{
    /// <summary>
    /// Representa un Dto de la aplicación
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IApplicationDto<T> where T : Entity<T>
    {
        /// <summary>
        /// Setea los valores para una entidad
        /// </summary>
        /// <param name="entity"></param>
        void SetValues(T entity);

        /// <summary>
        /// Devuelve una entidad a partir de los valores del dto.
        /// </summary>
        /// <returns></returns>
        T ToEntity();
    }

    public static class ApplicationDto
    {
        public static T UpdateOrCreate<T>(T existing, T from, Action<T, T> modify) where T : Entity<T>
        {
            if(from == null) return existing;
            if(existing == null) return from;
            modify(existing, from);
            return existing;
        }

        public static T UpdateOrCreateFromDto<T, TDto>(T existing, TDto from, Action<T, TDto> modify) where TDto : class, IApplicationDto<T> where T : Entity<T>
        {
            if(from == null) return existing;
            if(existing == null) return from.ToEntity();
            modify(existing, from);
            return existing;
        }
    }
}
