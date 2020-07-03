using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.Domain.Core
{
    /// <summary>
    /// Entidad con propiedades para la auditoría
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AuditEntity<T> : DatedEntity<T> where T : AuditEntity<T>
    {
        /// <summary>
        /// Propiedad que representa el usuario que modifica la entidad
        /// </summary>
        public String ModifiedBy { get; set; }

        /// <summary>
        /// Propiedad que representa el usuario quen creó la entidad. 
        /// </summary>
        public String CreatedBy { get; set; }

        /// <summary>
        /// Propiedad que representa la eliminación de un elemento. 
        /// </summary>
        public virtual DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Propiedad que representa si el usuario  fue eliminado o no. Se hace eliminación por baja lógica. 
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
