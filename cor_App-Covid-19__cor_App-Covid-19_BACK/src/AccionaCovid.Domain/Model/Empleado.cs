using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class Empleado : Entity<Empleado>
    {
        public Empleado()
        {
            AlertaEmpleado = new HashSet<AlertaEmpleado>();
            AlertaServiciosMedicos = new HashSet<AlertaServiciosMedicos>();
            EmpleadoRole = new HashSet<EmpleadoRole>();
            EntregaEquipoProteccion = new HashSet<EntregaEquipoProteccion>();
            FichaLaboral = new HashSet<FichaLaboral>();
            LocalizacionEmpleados = new HashSet<LocalizacionEmpleados>();
            Pasaporte = new HashSet<Pasaporte>();
            AspNetUsers = new HashSet<AspNetUsers>();
            AmbitoAccesoEmpleadoPais = new HashSet<AmbitoAccesoEmpleadoPais>();
            AmbitoAccesoEmpleadoRegion = new HashSet<AmbitoAccesoEmpleadoRegion>();
            AmbitoAccesoEmpleadoArea = new HashSet<AmbitoAccesoEmpleadoArea>();
            AdjudicacionTrabajoObra = new HashSet<AdjudicacionTrabajoObra>();
        }

        public int? IdFichaLaboral { get; set; }
        public int? IdFichaMedica { get; set; }
        public int? IdIntegracionExternos { get; set; }
        public string Nombre { get; set; }
        public long? NumEmpleado { get; set; }
        public int? Genero { get; set; }
        public DateTime UltimaModif { get; set; }
        public string Apellido { get; set; }
        public int? IdUsuarioWorkDay { get; set; }
        public string Mail { get; set; }
        public string Nif { get; set; }
        public string Telefono { get; set; }
        public bool? Bloqueado { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Upn { get; set; }
        public bool? EsServicioMedico { get; set; }
        public bool? InterAcciona { get; set; }

        public int? ManualCreator { get; set; }


        public virtual FichaLaboral IdFichaLaboralNavigation { get; set; }
        public virtual FichaMedica IdFichaMedicaNavigation { get; set; }
        public virtual UsuarioWorkDay IdUsuarioWorkDayNavigation { get; set; }
        public virtual ICollection<AlertaEmpleado> AlertaEmpleado { get; set; }
        public virtual ICollection<AlertaServiciosMedicos> AlertaServiciosMedicos { get; set; }
        public virtual ICollection<EmpleadoRole> EmpleadoRole { get; set; }
        public virtual ICollection<EntregaEquipoProteccion> EntregaEquipoProteccion { get; set; }
        public virtual ICollection<FichaLaboral> FichaLaboral { get; set; }
        public virtual ICollection<LocalizacionEmpleados> LocalizacionEmpleados { get; set; }
        public virtual ICollection<Pasaporte> Pasaporte { get; set; }
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
        public virtual ICollection<AmbitoAccesoEmpleadoPais> AmbitoAccesoEmpleadoPais { get; set; }
        public virtual ICollection<AmbitoAccesoEmpleadoRegion> AmbitoAccesoEmpleadoRegion { get; set; }
        public virtual ICollection<AmbitoAccesoEmpleadoArea> AmbitoAccesoEmpleadoArea { get; set; }
        public virtual ICollection<AdjudicacionTrabajoObra> AdjudicacionTrabajoObra { get; set; }
        public virtual IntegracionExternos IntegracionExternos { get; set; }
    }
}
