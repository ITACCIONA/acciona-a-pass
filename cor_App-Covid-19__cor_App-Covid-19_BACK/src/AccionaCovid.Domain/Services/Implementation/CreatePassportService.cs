using AccionaCovid.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccionaCovid.Domain.Services
{
    /// <summary>
    /// Servicio de creacion de un passport
    /// </summary>
    public class CreatePassportService : ICreatePassportService
    {
        /// <summary>
        /// Evento de calculo del nuevo estado
        /// </summary>
        public event EventHandler AddNewPassportEvent;

        /// <summary>
        /// Evento de calculo del nuevo estado
        /// </summary>
        public event EventHandler AddOldPassportEvent;

        public event EventHandler CalculateNewStateEvent;

        public StateMatrizData Matrix { get; set; }

        /// <summary>
        /// Crea un nuevo pasaporte para el empleadoa partir de su información actual e histórica.
        /// </summary>
        /// <param name="empleado">Empleado.</param>
        /// <param name="localDateOfRegistry">Fecha.</param>
        /// <param name="estados">Estados</param>
        /// <param name="currentEstadoPasaporte">Estado actual del pasaporte</param>
        /// <param name="pcrList">Listado de resultados PCR</param>
        /// <param name="lastAnalitIgG">Último resultado de a´nalítica IgG</param>
        /// <param name="lastTestRapido">Último test rápido</param>
        /// <param name="lastFiebre">Última valoración de fiebre</param>
        /// <param name="utiDeclFiebre">Última declaració de fiebre</param>
        /// <param name="utiDeclOtros">Última declaración de otros síntomas</param>
        /// <param name="utiDeclContato">Última declaración de contacto</param>
        public void CreateWithStatedCalculated(Empleado empleado, DateTimeOffset localDateOfRegistry, Dictionary<string, EstadoPasaporte> estados, EstadoPasaporte currentEstadoPasaporte,
            List<ResultadoTestPcr> pcrList, ValoracionParametroMedico lastAnalitIgG, ValoracionParametroMedico lastAnalitIgM, ResultadoTestMedico lastTestRapido, ValoracionParametroMedico lastFiebre,
            ResultadoEncuestaSintomas utiDeclFiebre, ResultadoEncuestaSintomas utiDeclOtros, ResultadoEncuestaSintomas utiDeclContato)
        {

            StateMatrizData matriz = new StateMatrizData(pcrList, lastAnalitIgG, lastAnalitIgM, lastTestRapido, lastFiebre, utiDeclFiebre, utiDeclOtros, utiDeclContato);
            EstadoPasaporte newEstadoPasaporte = matriz.Calculate(estados);

            // lanzo evento
            OnCalculateNewStateEvent(new CalculateNewStateEventArgs() { Matrix = matriz });

            /// En una transición calculada no se puede ir a mejor según las prioridades de estado
            
            if ((currentEstadoPasaporte?.IdColorEstadoNavigation?.Prioridad ?? 9999) < newEstadoPasaporte.IdColorEstadoNavigation.Prioridad)
            {
                // Mejora el color ---> viejo estado
                CreateFromChoosenState(empleado, localDateOfRegistry, currentEstadoPasaporte, false);
                OnAddOldPassportEventt(new EventArgs());
                return;
            }

            if ((currentEstadoPasaporte?.IdTipoEstadoNavigation?.Prioridad ?? 9999) < newEstadoPasaporte.IdTipoEstadoNavigation.Prioridad)
            {
                // Mejora el tipo de estado ---> viejo estado
                CreateFromChoosenState(empleado, localDateOfRegistry, currentEstadoPasaporte, false);
                OnAddOldPassportEventt(new EventArgs());
                return;
            }

            // Empeora el color -> nuevo estado
            // Se mantiene el color, empeora tipo de estado -> nuevo estado
            // Se mantiene el color, se mantiene le tipo de estado -> nuevo estado
            CreateFromChoosenState(empleado, localDateOfRegistry, newEstadoPasaporte, false);
            OnAddNewPassportEvent(new EventArgs());
        }

        /// <summary>
        /// Crea un pasaporte para un empleado dado
        /// </summary>
        /// <param name="empleado"></param>
        public void CreateInitState(Empleado empleado, DateTimeOffset localDateOfRegistry, List<EstadoPasaporte> estados)
        {
            if (!empleado.Pasaporte.Any())
            {
                EstadoPasaporte iniState = estados.FirstOrDefault(c => c.Nombre == "EstadoInicial");
                CreateFromChoosenState(empleado, localDateOfRegistry, iniState, false);
            }
        }

        /// <summary>
        /// Creacion de un pasaporte a partir de un estado. Si se necesita usar una lógica diferente, crear otro método.
        /// Se debe hacer include de Pasaporte, hay que marcarlos como inactivos.
        /// </summary>
        /// <param name="empleado"></param>
        /// <param name="localDateOfRegistry"></param>
        /// <param name="idPassportState"></param>
        public void CreateFromChoosenState(Empleado empleado, DateTimeOffset localDateOfRegistry, EstadoPasaporte passportState, bool isManual)
        {
            Pasaporte pasaporte = new Pasaporte()
            {
                FechaCreacion = localDateOfRegistry,
                Activo = true,
                IdEstadoPasaporte = passportState.Id,
                IsManual = isManual
            };

            int? diasValidez = passportState.DiasValidez;

            if (diasValidez != null)
            {
                pasaporte.FechaExpiracion = new DateTimeOffset(localDateOfRegistry.Year, localDateOfRegistry.Month, localDateOfRegistry.Day, 00, 00, 00, localDateOfRegistry.Offset).AddDays(diasValidez.Value);
            }

            foreach (var p in empleado.Pasaporte)
            {
                p.Activo = false;
            }

            empleado.Pasaporte.Add(pasaporte);
        }

        #region Events

        protected virtual void OnCalculateNewStateEvent(EventArgs e)
        {
            EventHandler handler = CalculateNewStateEvent;
            handler?.Invoke(this, e);
        }

        protected virtual void OnAddNewPassportEvent(EventArgs e)
        {
            EventHandler handler = AddNewPassportEvent;
            handler?.Invoke(this, e);
        }

        protected virtual void OnAddOldPassportEventt(EventArgs e)
        {
            EventHandler handler = AddOldPassportEvent;
            handler?.Invoke(this, e);
        }
        #endregion
    }

    /// <summary>
    /// Argumento del evento del calculo
    /// </summary>
    public class CalculateNewStateEventArgs : EventArgs
    {
        public StateMatrizData Matrix { get; set; }
    }
}
