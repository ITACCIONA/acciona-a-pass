using System.Threading.Tasks;

namespace AccionaCovid.Identity.Infrastructure
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string email, string subject, string htmlMessage);
    }
}