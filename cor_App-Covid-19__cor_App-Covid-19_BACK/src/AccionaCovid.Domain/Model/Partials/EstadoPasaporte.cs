namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase partial
    /// </summary>
    public partial class EstadoPasaporte
    {
        public const int ExpiredEstadoId = 26;

        public enum AllowedRRHHPassportStatesId
        {
            AsintomaticoPcrReconvertidoIGG = 3,
            SintomaticoPcrReconvertidoIGG = 10
        }

        public enum PapertStatesId
        {
            NoSintomaticoPaper = 39,
            SintomaticoPaper = 40,
        }
    }
}
