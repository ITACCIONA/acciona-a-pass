using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccionaCovid.Domain.Model
{
    public partial class Empleado 
    {
        public Empleado()
        {
            EmpleadoRole = new HashSet<EmpleadoRole>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        public int? IdFichaLaboral { get; set; }
        public int? IdFichaMedica { get; set; }
        public string Nombre { get; set; }
        public long? NumEmpleado { get; set; }
        public int? Genero { get; set; }
        public DateTime UltimaModif { get; set; }
        public string Apellido { get; set; }
        public string Mail { get; set; }
        public string Nif { get; set; }
        public string Telefono { get; set; }
        public bool? Bloqueado { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Upn { get; set; }
        public bool? EsServicioMedico { get; set; }
        public bool? InterAcciona { get; set; }

        /// <summary>
        /// Propiedad que indica si el registro tiene un borrado logico
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Ultima accion realizada sobre el registro
        /// </summary>
        public string LastAction { get; set; }

        /// <summary>
        /// Fecha de la ultima accion
        /// </summary>
        public DateTime LastActionDate { get; set; }

        /// <summary>
        /// Identificador del usuario que ha realizado la ultima accion
        /// </summary>
        public int IdUser { get; set; }

        public int? IdUsuarioWorkDay { get; set; }
        public virtual UsuarioWorkDay IdUsuarioWorkDayNavigation { get; set; }

        public virtual ICollection<EmpleadoRole> EmpleadoRole { get; set; }

        public virtual FichaMedica IdFichaMedicaNavigation { get; set; }

        public virtual FichaLaboral IdFichaLaboralNavigation { get; set; }
    }
}
