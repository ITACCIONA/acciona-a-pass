using System;
namespace Acciona.Domain.Utils
{
    public class QRUtils
    {
        public static string GenerateQRInfo(Domain.Model.Employee.Passport passport)
        {
            long date = passport.FechaExpiracion.HasValue ? passport.FechaExpiracion.Value.Ticks : -1;
            if (!passport.FechaExpiracion.HasValue)
                date = -1;
            String values = String.Format("{0};{1};{2}", passport.IdEmpleado,
                passport.ColorPasaporte, date);
            return EncryptUtils.Encriptar(values);//JsonConvert.SerializeObject(info)));
        }
    }
}
