using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase empleado
    /// </summary>
    public partial class Empleado
    {
        /// <summary>
        /// Enumerado que indica los tipos codigo para los bloques especiales
        /// </summary>
        public enum Gender
        {
            Mujer = 1,
            Hombre = 2,
            Indefinido = 3
        }

        /// <summary>
        /// Propiedad que indica el nombre completo del usuario
        /// </summary>
        [NotMapped]
        public string NombreCompleto
        {
            get
            {
                if (string.IsNullOrEmpty(Apellido))
                {
                    return Nombre;
                }
                else
                {
                    return $"{Nombre} {Apellido}";
                }
            }
        }

        /// <summary>
        /// Propiedad que indica el nombre completo del usuario
        /// </summary>
        [NotMapped]
        public string Iniciales
        {
            get
            {
                string app = string.IsNullOrEmpty(Apellido) ? "-" : Apellido.ToUpper();
                return $"{Nombre.ToUpper()[0]}{app[0]}";
            }
        }

        public Empleado(UsuarioWorkDay usuWorkday)
        {
            this.Nombre = usuWorkday.Nombre;
            this.Apellido = $"{usuWorkday.Apellido1} {usuWorkday.Apellido2}".Trim();
            this.FechaNacimiento = usuWorkday.FechaNacimiento;
            this.Genero = usuWorkday.Genero;
            this.Nif = usuWorkday.Nif;
            this.Mail = usuWorkday.Mail;
            this.Telefono = usuWorkday.Telefono;
            this.NumEmpleado = usuWorkday.NumEmpleado ?? usuWorkday.IdWorkDay;
            this.UltimaModif = usuWorkday.UltimaModif;
            this.Bloqueado = false;
            this.EsServicioMedico = false;
            this.InterAcciona = usuWorkday.InterAcciona;
            this.Upn = usuWorkday.Upn;
            this.LastAction = "CREATE";
            this.LastActionDate = DateTime.UtcNow;
            this.IdUsuarioWorkDayNavigation = usuWorkday;
            usuWorkday.Empleado = new List<Empleado>() { this };

            this.IdFichaLaboralNavigation = new FichaLaboral();
            this.IdFichaLaboralNavigation.MailProf = usuWorkday.MailCorporativo;
            this.IdFichaLaboralNavigation.TelefonoCorp = usuWorkday.TelefonoCorporativo;
            this.IdFichaLaboralNavigation.LastAction = "CREATE";
            this.IdFichaLaboralNavigation.LastActionDate = DateTime.UtcNow;
            this.IdFichaLaboralNavigation.Empleado.Add(this);

            this.IdFichaMedicaNavigation = new FichaMedica();
            this.IdFichaMedicaNavigation.FechaAlta = DateTime.Now;
            this.IdFichaMedicaNavigation.LastAction = "CREATE";
            this.IdFichaMedicaNavigation.LastActionDate = DateTime.UtcNow;
            this.IdFichaMedicaNavigation.Empleado.Add(this);

            this.EmpleadoRole = new List<EmpleadoRole>();
        }

        public Empleado(IntegracionExternos  integracionExterno)
        {
            this.Nombre = integracionExterno.Nombre;
            this.Apellido = $"{integracionExterno.Apellido1} {integracionExterno.Apellido2}".Trim();
            this.Nif = integracionExterno.Nif;
            this.UltimaModif = integracionExterno.UltimaModif;
            this.Bloqueado = false;
            this.EsServicioMedico = false;
            this.LastAction = "CREATE";
            this.LastActionDate = DateTime.UtcNow;
            this.IntegracionExternos = integracionExterno;
            integracionExterno.Empleado = new List<Empleado>() { this };

            this.IdFichaLaboralNavigation = new FichaLaboral();
            this.IdFichaLaboralNavigation.IsExternal = true;
            this.IdFichaLaboralNavigation.LastAction = "CREATE";
            this.IdFichaLaboralNavigation.LastActionDate = DateTime.UtcNow;
            this.IdFichaLaboralNavigation.Empleado.Add(this);

            this.IdFichaMedicaNavigation = new FichaMedica();
            this.IdFichaMedicaNavigation.FechaAlta = DateTime.Now;
            this.IdFichaMedicaNavigation.LastAction = "CREATE";
            this.IdFichaMedicaNavigation.LastActionDate = DateTime.UtcNow;
            this.IdFichaMedicaNavigation.Empleado.Add(this);

            this.EmpleadoRole = new List<EmpleadoRole>();
        }

        /// <summary>
        /// Metodo que calcula la edad del empleado en base a su fecha de nacimiento
        /// </summary>
        /// <param name="fechaNacimiento"></param>
        /// <returns></returns>
        public int CalcularEdad()
        {
            // Obtiene la fecha actual:
            DateTime fechaActual = DateTime.Today;

            // Comprueba que la se haya introducido una fecha válida; si 
            // la fecha de nacimiento es mayor a la fecha actual se muestra mensaje 
            // de advertencia:
            if (!FechaNacimiento.HasValue || FechaNacimiento.Value > fechaActual)
            {
                return -1;
            }
            else
            {
                int edad = fechaActual.Year - FechaNacimiento.Value.Year;

                // Comprueba que el mes de la fecha de nacimiento es mayor 
                // que el mes de la fecha actual:
                if (FechaNacimiento.Value.Month > fechaActual.Month)
                {
                    --edad;
                }

                return edad;
            }
        }
    }
}
