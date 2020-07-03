using Acciona.Domain.Model.Admin;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Model.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Domain.Repository
{
    public interface ISecurityRepository
    {
        Task<Passport> GetSecurityPassport(int idEmpleado);
        Task SendTemperature(int idEmpleado, string idDevice, bool isTemperatureOverThreshold, DateTime dateTime);
        Task<UserPaper> GetUserPaper(String document, string phone);
        Task GenerateManual(int idEmpleado, bool isGreenPaper, DateTime dateTime);
        Task<IEnumerable<Location>> GetLocations();
    }
}
