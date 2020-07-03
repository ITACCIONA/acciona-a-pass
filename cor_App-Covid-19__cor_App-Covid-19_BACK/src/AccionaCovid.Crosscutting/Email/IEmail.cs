namespace AccionaCovid.Crosscutting
{
    /// <summary>
    /// Interfaz para el envío de correos electrónicos
    /// </summary>
    public interface IEmail
    {
        /// <summary>
        /// Método para el envío de correos electrónicos
        /// </summary>
        /// <param name="message"></param>
        public void SendMail(Message message);
    }
}