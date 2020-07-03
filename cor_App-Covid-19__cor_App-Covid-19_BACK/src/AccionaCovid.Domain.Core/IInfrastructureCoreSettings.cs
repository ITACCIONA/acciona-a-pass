namespace AccionaCovid.Domain.Core
{
    /// <summary>
    /// Opciones de configuración a nivel de dominio de la aplicación
    /// </summary>
    public class InfrastructureCoreSettings
    {
        /// <summary>
        /// Indica si el sistema de persistencia funciona mediante borrado lógico
        /// </summary>
        public bool LogicalRemove { get; set; }
    }
}