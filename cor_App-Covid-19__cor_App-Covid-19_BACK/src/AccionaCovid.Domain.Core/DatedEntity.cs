using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.Domain.Core
{
    public abstract class DatedEntity<T> : Entity<T> where T : DatedEntity<T>
    {
        /// <summary>
        /// Propiedad que representa la fecha de creación.
        /// </summary>
        public virtual DateTime? CreationDate { get; set; }

        /// <summary>
        /// Propiedad que representa la fecha de modificación. 
        /// </summary>
        public virtual DateTime? ModificationDate { get; set; }
    }
}
