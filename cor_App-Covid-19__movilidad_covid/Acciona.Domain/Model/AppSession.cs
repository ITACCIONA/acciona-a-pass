using Acciona.Domain.Model.Admin;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Model.Security;

namespace Acciona.Domain.Model
{
    public class AppSession
    {        
        public string AccesToken { get; set; }
        public string TokenType { get; set; }
        public string User { get; set; }
        public string Language { get; set; }
        public User UserInfo { get; set; }

        public Location Location { get; set; }

        //Estado entre pantalla
        public Alert SelectedAlert { get; set; }
        public QRInfo QRInfo { get; set; }
        public bool Panic { get; set; }


    }
}
