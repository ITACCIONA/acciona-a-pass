using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase UsuarioWorkDay
    /// </summary>
    public partial class UsuarioWorkDay
    {
        /// <summary>
        /// Índice del identificador workday en el CSV
        /// </summary>
        private static int idWorkdayIndex;

        /// <summary>
        /// Índice del nombre en el CSV
        /// </summary>
        private static int nombreIndex;

        /// <summary>
        /// Índice del apellido1 en el CSV
        /// </summary>
        private static int apellido1Index;

        /// <summary>
        /// Índice del apellido2 en el CSV
        /// </summary>
        private static int apellido2Index;

        /// <summary>
        /// Índice del fecha nacimiento en el CSV
        /// </summary>
        private static int fechaNacimientoIndex;

        /// <summary>
        /// Índice del genero en el CSV
        /// </summary>
        private static int generoIndex;

        /// <summary>
        /// Índice del nif en el CSV
        /// </summary>
        private static int nifIndex;

        /// <summary>
        /// Índice del mail en el CSV
        /// </summary>
        private static int mailIndex;

        /// <summary>
        /// Índice del telefono en el CSV
        /// </summary>
        private static int telefonoIndex;

        /// <summary>
        /// Índice del division en el CSV
        /// </summary>
        private static int divisionIndex;

        /// <summary>
        /// Índice del departamento en el CSV
        /// </summary>
        private static int departamentoIndex;

        /// <summary>
        /// Índice del localizacion en el CSV
        /// </summary>
        private static int localizacionIndex;

        /// <summary>
        /// Índice del responsable en el CSV
        /// </summary>
        private static int idResponsableIndex;

        /// <summary>
        /// Índice del mail corporativo en el CSV
        /// </summary>
        private static int mailCorporativoIndex;

        /// <summary>
        /// Índice del telefono corporativo en el CSV
        /// </summary>
        private static int telefonoCorporativoIndex;

        /// <summary>
        /// Índice del upn en el CSV
        /// </summary>
        private static int upnIndex;

        /// <summary>
        /// Índice de interacciona en el CSV
        /// </summary>
        private static int interaccionaIndex;

        /// <summary>
        /// Índice de interacciona en el CSV
        /// </summary>
        private static int tecnologiaIndex;

        /// <summary>
        /// Índice de la acción de importación a realizar
        /// </summary>
        private static int importActionIndex;

        /// <summary>
        /// Ación a realizar al importar el dato
        /// </summary>
        [NotMapped]
        public string ImportAction { get; set; }

        /// <summary>
        /// Define los índices de las propiedades a partir de la cabecera del CSV
        /// </summary>
        /// <param name="headers">Cabecera del CSV</param>
        public static void SetPropertyIndexes(string[] headers)
        {
            idWorkdayIndex = Array.IndexOf(headers, "Employee_ID");
            nombreIndex = Array.IndexOf(headers, "Nombre");
            apellido1Index = Array.IndexOf(headers, "Apellido1");
            apellido2Index = Array.IndexOf(headers, "Apellido2");
            fechaNacimientoIndex = Array.IndexOf(headers, "Fecha_Nac");
            generoIndex = Array.IndexOf(headers, "Genero");
            nifIndex = Array.IndexOf(headers, "NIF");
            mailIndex = Array.IndexOf(headers, "Correo_Per");
            telefonoIndex = Array.IndexOf(headers, "Telefono_Per");
            divisionIndex = Array.IndexOf(headers, "Negocio_Division");
            departamentoIndex = Array.IndexOf(headers, "Supervisory");
            localizacionIndex = Array.IndexOf(headers, "Location");
            idResponsableIndex = Array.IndexOf(headers, "Responsable");
            mailCorporativoIndex = Array.IndexOf(headers, "Correo_Prof");
            telefonoCorporativoIndex = Array.IndexOf(headers, "Telefono_Corp");
            upnIndex = Array.IndexOf(headers, "UserName");
            interaccionaIndex = Array.IndexOf(headers, "Status");
            tecnologiaIndex = Array.IndexOf(headers, "Tecnologia");
            importActionIndex = Array.IndexOf(headers, OpcionesIntegracion.IntegracionHeaderField);
        }

        /// <summary>
        /// Crea una instancia en función de la fila de datos del CSV
        /// </summary>
        /// <param name="data"></param>
        public UsuarioWorkDay(string[] data)
        {
            try { this.IdWorkDay = UsuarioWorkDay.idWorkdayIndex >= 0 ? Convert.ToInt64(data[UsuarioWorkDay.idWorkdayIndex]) : 0; } catch (Exception ex) { throw new Exception($"Incorrect format for field {nameof(this.IdWorkDay)}", ex); }
            this.ImportAction = UsuarioWorkDay.importActionIndex >= 0 ? data[UsuarioWorkDay.importActionIndex] : null;

            if (this.ImportAction != OpcionesIntegracion.IntegracionDeleteAction)
            {
                this.Nombre = UsuarioWorkDay.nombreIndex >= 0 ? data[UsuarioWorkDay.nombreIndex] : null;
                this.Apellido1 = UsuarioWorkDay.apellido1Index >= 0 ? data[UsuarioWorkDay.apellido1Index] : null;
                this.Apellido2 = UsuarioWorkDay.apellido2Index >= 0 ? data[UsuarioWorkDay.apellido2Index] : null;
                try { this.FechaNacimiento = UsuarioWorkDay.fechaNacimientoIndex >= 0 && !string.IsNullOrWhiteSpace(data[UsuarioWorkDay.fechaNacimientoIndex]) ? (DateTime?)DateTime.ParseExact(data[UsuarioWorkDay.fechaNacimientoIndex], "yyyy/MM/dd", CultureInfo.InvariantCulture) : null; } catch (Exception ex) { throw new Exception($"Incorrect format for field {nameof(this.FechaNacimiento)}", ex); }
                if (this.FechaNacimiento < System.Data.SqlTypes.SqlDateTime.MinValue.Value.AddHours(12))
                {
                    this.FechaNacimiento = null;
                }
                this.Genero = UsuarioWorkDay.generoIndex >= 0 ? (int?)(data[UsuarioWorkDay.generoIndex] == "Female" ? 1 : 2) : null;
                this.Nif = UsuarioWorkDay.nifIndex >= 0 ? data[UsuarioWorkDay.nifIndex] : null;
                this.Mail = UsuarioWorkDay.mailIndex >= 0 ? data[UsuarioWorkDay.mailIndex] : null;
                this.Telefono = UsuarioWorkDay.telefonoIndex >= 0 ? data[UsuarioWorkDay.telefonoIndex] : null;
                try { this.Division = UsuarioWorkDay.divisionIndex >= 0 ? Convert.ToInt64(data[UsuarioWorkDay.divisionIndex]) : 0; } catch (Exception ex) { throw new Exception($"Incorrect format for field {nameof(this.Division)}", ex); }
                try { this.Departamento = UsuarioWorkDay.departamentoIndex >= 0 ? Convert.ToInt64(data[UsuarioWorkDay.departamentoIndex]) : 0; } catch (Exception ex) { throw new Exception($"Incorrect format for field {nameof(this.Departamento)}", ex); }
                this.Localizacion = UsuarioWorkDay.localizacionIndex >= 0 ? data[UsuarioWorkDay.localizacionIndex] : null;
                try { this.IdResponsable = UsuarioWorkDay.idResponsableIndex >= 0 ? Convert.ToInt64(data[UsuarioWorkDay.idResponsableIndex]) : 0; } catch (Exception ex) { throw new Exception($"Incorrect format for field {nameof(this.IdResponsable)}", ex); }
                this.MailCorporativo = UsuarioWorkDay.mailCorporativoIndex >= 0 ? data[UsuarioWorkDay.mailCorporativoIndex] : null;
                this.TelefonoCorporativo = UsuarioWorkDay.telefonoCorporativoIndex >= 0 ? data[UsuarioWorkDay.telefonoCorporativoIndex] : null;
                this.UltimaModif = DateTime.UtcNow;
                this.EsServicioMedico = false;
                this.Upn = UsuarioWorkDay.upnIndex >= 0 ? data[UsuarioWorkDay.upnIndex] : null;
                this.InterAcciona = UsuarioWorkDay.interaccionaIndex >= 0 && data[UsuarioWorkDay.interaccionaIndex].ToUpper() == "ALTA";
                this.Tecnologia = UsuarioWorkDay.tecnologiaIndex >= 0 ? data[UsuarioWorkDay.tecnologiaIndex] : null;
            }
            this.LastAction = "CREATE";
            this.LastActionDate = DateTime.UtcNow;

        }
    }
}
