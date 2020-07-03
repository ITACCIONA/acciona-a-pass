using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace AccionaCovid.Identity.Infrastructure
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridClient client;
        private readonly EmailAddress from;
        private readonly ILogger<SendGridEmailSender> logger;

        public SendGridEmailSender(SendGridClient client, EmailAddress from, ILogger<SendGridEmailSender> logger)
        {
            this.client = client;
            this.from = from;
            this.logger = logger;
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SendGridMessage msg = MailHelper.CreateSingleEmail(this.from, new EmailAddress(email), subject, null, htmlMessage);
            Response response = await client.SendEmailAsync(msg).ConfigureAwait(false);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                this.logger.LogError("Couldn't send email. SendGrid response: {Response}", response);
                return false;
            }

            return true;
        }
    }
}
