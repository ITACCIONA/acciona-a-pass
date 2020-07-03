namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase partial
    /// </summary>
    public partial class FactorRiesgo
    {
        /// <summary>
        /// Enumerado que indica los tipos de parametros disponibles
        /// </summary>
        public enum ParameterTypes
        {
            AltaExposicion = 1,
            Vulnerables = 2,
            Positivo = 3
        }
    }
}
