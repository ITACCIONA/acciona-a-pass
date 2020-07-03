using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccionaCovid.Domain.Core
{
    /// <summary>
    /// Clase que representa una Entidad del Dominio
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Entity<T> where T : Entity<T>
    {
        #region Propiedades

        /// <summary>
        /// Identificador unívoco.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        /// <summary>
        /// Propiedad que indica si el registro tiene un borrado logico
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Ultima accion realizada sobre el registro
        /// </summary>
        [MaxLength(10)]
        [Required]
        [DefaultValue("(N'CREATE')")]
        public string LastAction { get; set; }

        /// <summary>
        /// Fecha de la ultima accion
        /// </summary>
        [DefaultValue("(getdate())")]
        [Column(TypeName = "datetime")]
        public DateTime LastActionDate { get; set; }

        /// <summary>
        /// Identificador del usuario que ha realizado la ultima accion
        /// </summary>
        public int IdUser { get; set; }

        /// <summary>
        /// Nombre del usuario que ha realizado la ultima accion
        /// </summary>
        public string UserName { get; set; }

        #endregion

        #region Override
        public bool IsTransient()
        {
            return this.Id == default(Int32);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<T>))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetHashCode() != obj.GetHashCode())
                return false;

            Entity<T> item = (Entity<T>)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;

            return item.Id == this.Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
                return this.Id.GetHashCode() ^ 31;

            return base.GetHashCode();

        }

        public static bool operator ==(Entity<T> left, Entity<T> right)
        {
            if (Object.Equals(left, null))
                return Object.Equals(right, null);
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity<T> left, Entity<T> right)
        {
            return !(left == right);
        }

        #endregion
    }
}
