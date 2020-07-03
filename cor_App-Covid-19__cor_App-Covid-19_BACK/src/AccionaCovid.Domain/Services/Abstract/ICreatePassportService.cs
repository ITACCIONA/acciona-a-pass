using AccionaCovid.Domain.Model;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Services
{
    public interface ICreatePassportService
    {
        event EventHandler CalculateNewStateEvent;

        event EventHandler AddNewPassportEvent;

        event EventHandler AddOldPassportEvent;

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
        void CreateWithStatedCalculated(Empleado empleado, DateTimeOffset localDateOfRegistry, Dictionary<string, EstadoPasaporte> estados, EstadoPasaporte currentEstadoPasaporte,
            List<ResultadoTestPcr> pcrList, ValoracionParametroMedico lastAnalitIgG, ValoracionParametroMedico lastAnalitIgM, ResultadoTestMedico lastTestRapido, ValoracionParametroMedico lastFiebre,
            ResultadoEncuestaSintomas utiDeclFiebre, ResultadoEncuestaSintomas utiDeclOtros, ResultadoEncuestaSintomas utiDeclContato);

        /// <summary>
        /// Crea un pasaporte para un empleado dado
        /// </summary>
        /// <param name="empleado"></param>
        void CreateInitState(Empleado empleado, DateTimeOffset localDateOfRegistry, List<EstadoPasaporte> estados);

        /// <summary>
        /// Crea un pasaporte para un empleado dado
        /// </summary>
        /// <param name="empleado"></param>
        void CreateFromChoosenState(Empleado empleado, DateTimeOffset localDateOfRegistry, EstadoPasaporte passportState, bool isManual);
    }
}