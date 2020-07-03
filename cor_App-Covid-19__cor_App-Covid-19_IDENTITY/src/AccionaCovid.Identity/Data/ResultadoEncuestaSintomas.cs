using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccionaCovid.Domain.Model
{
    public partial class ResultadoEncuestaSintomas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public int IdFichaMedica { get; set; }
        public int IdTipoSintoma { get; set; }
        public bool Valor { get; set; }

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

        public virtual FichaMedica IdFichaMedicaNavigation { get; set; }
    }
}
