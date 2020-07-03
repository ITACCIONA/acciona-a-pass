using Acciona.Data.Model;
using Acciona.Data.Model.Admin;
using Acciona.Data.Model.Employee;
using Acciona.Data.Model.Mapper;
using Acciona.Data.Model.Security;
using Acciona.Data.Repository.Base;
using Acciona.Domain.Model.Admin;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Acciona.Data.Repository
{
    public class EmployeeRepository : ApiBaseService, IEmployeeRepository
    {
        public async Task<IEnumerable<Alert>> GetAlerts()
        {
            string url = String.Concat(DataConstants.Endpoint,DataConstants.AlertsURL);

            var response = await MakeSessionHttpCall<BaseResponse<AlertsResponse>, string>(url, HttpVerbMethod.Get, null)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info.Select(x=>x.message));
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }
            return EmployeeMapper.MapAlertsResponse(response.data.ElementAt(0));
        }

        public async Task MarkAlertRead(int idAlert)
        {
            string url = String.Format(String.Concat(DataConstants.Endpoint, DataConstants.AlertReadURL),idAlert);

            var response = await MakeSessionHttpCall<BaseResponse, string>(url, HttpVerbMethod.Put, null)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info.Select(x=>x.message));
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }
        }

        public async Task<Passport> GetPassport()
        {
            string url = String.Concat(DataConstants.Endpoint, DataConstants.PassportURL);

            var response = await MakeSessionHttpCall<BaseResponse<PassportData>, string>(url, HttpVerbMethod.Get, null)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info.Select(x=>x.message));
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }
            return EmployeeMapper.MapPassport(response.data.ElementAt(0));
        }

        public async Task<Ficha> GetFicha()
        {
            string url = String.Concat(DataConstants.Endpoint, DataConstants.FichaURL);

            var response = await MakeSessionHttpCall<BaseResponse<FichaData>, string>(url, HttpVerbMethod.Get, null)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info.Select(x=>x.message));
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }
            return EmployeeMapper.MapFicha(response.data.ElementAt(0));
        }

        public async Task SendSymptoms(int idEmpleado,DateTime fecha, IEnumerable<RequestValue> values)
        {
            string url = String.Concat(DataConstants.Endpoint, DataConstants.SendSymptomsURL);

            RequestSymptoms request = new RequestSymptoms()
            {
                idEmployee=idEmpleado,
                currentDeviceDateTime=fecha.ToString(EmployeeMapper.DATEFORMAT),
                values=EmployeeMapper.MapRequestValueDataList(values)
            };

            var response = await MakeSessionHttpCall<BaseResponse, RequestSymptoms>(url, HttpVerbMethod.Post, request)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info.Select(x=>x.message));
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }
        }

        public async Task SendRiskFactor(int idEmpleado, DateTime fecha, IEnumerable<RequestValue> values)
        {
            string url = String.Concat(DataConstants.Endpoint, DataConstants.SendRiskFactorURL);

            RequestRiskFactor request = new RequestRiskFactor()
            {
                idEmployee = idEmpleado,
                fechaFactor = fecha.ToString(EmployeeMapper.DATEFORMAT),
                riskFactorValues = EmployeeMapper.MapRequestRiskFactorValueDataList(values)
            };

            var response = await MakeSessionHttpCall<BaseResponse, RequestRiskFactor>(url, HttpVerbMethod.Post, request)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info.Select(x=>x.message));
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }
        }

        public async Task SendPanic(DateTime fecha)
        {
            string date = HttpUtility.UrlEncode(fecha.ToString(EmployeeMapper.DATEFORMAT));
            string url = String.Format(String.Concat(DataConstants.Endpoint, DataConstants.PanicURL),date);            
            /*RequestPanic request = new RequestPanic()
            {
                currentDeviceDateTime = fecha.ToString(EmployeeMapper.DATEFORMAT), 
            };*/

            var response = await MakeSessionHttpCall<BaseResponse, string>(url, HttpVerbMethod.Put, null)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info.Select(x=>x.message));
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }
        }

        public async Task ModifyFicha(int idEmpleado,string telephone,long? idLocalization)
        {            
            string url = String.Concat(DataConstants.Endpoint, DataConstants.ModifyFichaURL);
            var request = new RequestModifyFicha()
            {
                idEmployee=idEmpleado,
                telefonoEmpleado=telephone,
                idLocalizacion=idLocalization
            };

            var response = await MakeSessionHttpCall<BaseResponse, RequestModifyFicha>(url, HttpVerbMethod.Put, request)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info.Select(x=>x.message));
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }
        }
    }
}
