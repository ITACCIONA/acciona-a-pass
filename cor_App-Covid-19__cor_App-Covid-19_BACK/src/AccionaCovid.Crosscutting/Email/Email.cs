using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace AccionaCovid.Crosscutting
{
    /// <summary>
    /// Clase para el envío de correos electrónicos
    /// </summary>
    public class Email : IEmail
    {
        /// <summary>
        /// from   
        /// </summary>
        private readonly string from;

        /// <summary>
        /// UserName  
        /// </summary>
        private readonly string userName;

        /// <summary>
        /// Password  
        /// </summary>
        private readonly string password;

        /// <summary>
        /// Host  
        /// </summary>
        private readonly string host;

        /// <summary>
        /// Port  
        /// </summary>
        private readonly int port;

        /// <summary>
        /// EnableSsl   
        /// </summary>
        private readonly bool enableSsl;

        /// <summary>
        /// SmtpClient
        /// </summary>
        private SmtpClient client;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="from"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="enableSsl"></param>
        public Email(string from, string userName, string password, string host, int port, bool enableSsl = true)
        {
            this.from = from ?? throw new ArgumentNullException(nameof(from));
            this.userName = userName ?? throw new ArgumentNullException(nameof(userName));
            this.password = password ?? throw new ArgumentNullException(nameof(password));
            this.host = host ?? throw new ArgumentNullException(nameof(host));
            this.port = port;
            this.enableSsl = enableSsl;

            Configure();
        }

        /// <summary>
        ///  Configuración de servidor de envío
        /// </summary>
        private void Configure()
        {
            try
            {
                client = new SmtpClient()
                {
                    UseDefaultCredentials = false,
                    Host = host,
                    Port = port,
                    Credentials = new NetworkCredential(userName, password),
                    EnableSsl = enableSsl
                };
            }
            catch (Exception)
            {

               // throw;
            }
        }

        /// <summary>
        /// Método para el envío de correos electrónicos
        /// </summary>
        /// <param name="message"></param>
        public void SendMail(Message message)
        {
            try
            {
                if (message == null) throw new ArgumentNullException(nameof(message));
                if (message.To == null) throw new ArgumentNullException(nameof(message.To));

                #region Preparamos el correo para el envío

                MailMessage mail = new MailMessage()
                {
                    Subject = message.Subject,
                    Body = message.Body,
                    From = new MailAddress(from),
                    IsBodyHtml = message.IsBodyHtml
                };

                foreach (var item in message.To)
                {
                    mail.To.Add(item);
                }

                if (message.CC == null)
                {
                    foreach (var item in message.CC)
                    {
                        mail.CC.Add(item);
                    }
                }

                if (message.Bcc == null)
                {
                    foreach (var item in message.Bcc)
                    {
                        mail.Bcc.Add(item);
                    }
                }

                #endregion

                if (mail.To.Count > 0)
                {
                    //Enviamos el correo
                    client.Send(mail);
                }

            }
            catch (Exception)
            {
                //throw;
            }
        }
    }
    /// <summary>
    /// Correo electrónico a enviar
    /// </summary>
    public class Message
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public Message()
        {
            this.To = new List<string>();
            this.CC = new List<string>();
            this.Bcc = new List<string>();
        }

        /// <summary>
        /// From 
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// To 
        /// </summary>
        public List<string> To { get; set; }

        /// <summary>
        /// CC 
        /// </summary>
        public List<string> CC { get; set; }

        /// <summary>
        /// Bcc 
        /// </summary>
        public List<string> Bcc { get; set; }

        /// <summary>
        /// Subject 
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Body  
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// IsBodyHtml   
        /// </summary>
        public bool IsBodyHtml { get; set; }

    }
}