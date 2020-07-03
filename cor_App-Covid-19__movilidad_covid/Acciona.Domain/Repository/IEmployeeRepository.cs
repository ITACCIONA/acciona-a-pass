using Acciona.Domain.Model.Employee;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Domain.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Alert>> GetAlerts();
        Task MarkAlertRead(int idAlert);
        Task<Passport> GetPassport();
        Task<Ficha> GetFicha();
        Task SendSymptoms(int idEmpleado,DateTime fecha, IEnumerable<RequestValue> values);
        Task SendRiskFactor(int idEmpleado,DateTime fecha, IEnumerable<RequestValue> values);
        Task SendPanic(DateTime fecha);
        Task ModifyFicha(int idEmpleado, string telephone, long? idLocalization);
    }
}
