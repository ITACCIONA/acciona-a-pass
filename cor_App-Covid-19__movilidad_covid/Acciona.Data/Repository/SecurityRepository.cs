using Acciona.Data.Model;
using Acciona.Data.Model.Admin;
using Acciona.Data.Model.Employee;
using Acciona.Data.Model.Mapper;
using Acciona.Data.Model.Security;
using Acciona.Data.Repository.Base;
using Acciona.Domain.Model.Admin;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Model.Security;
using Acciona.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Acciona.Data.Repository
{
    public class SecurityRepository : ApiBaseService, ISecurityRepository
    {
        public async Task<Passport> GetSecurityPassport(int idEmpleado)
        {
            string url = String.Format(String.Concat(DataConstants.Endpoint, DataConstants.SecurityPassportURL),idEmpleado);

            var response = await MakeApiKeyHttpCall<BaseResponse<PassportData>, string>(url, HttpVerbMethod.Get, null)
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

        public async Task SendTemperature(int idEmpleado, string idDevice, bool isTemperatureOverThreshold, DateTime dateTime)
        {
            string url = String.Format(String.Concat(DataConstants.Endpoint, DataConstants.SendTemperatureURL),idEmpleado);

            var request = new SendTemperatureRequest()
            {
                idEmployee = idEmpleado,
                idDevice = idDevice,
                isTemperatureOverThreshold = isTemperatureOverThreshold,
                meditionDateTime = dateTime.ToLocalTime().ToString(EmployeeMapper.DATEFORMAT)
            };

            var response = await MakeApiKeyHttpCall<BaseResponse, SendTemperatureRequest>(url, HttpVerbMethod.Post, request)
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

        public async Task<UserPaper> GetUserPaper(String document,string phone)
        {
            string url = String.Format(String.Concat(DataConstants.Endpoint, DataConstants.SecurityManualURL),document);
            if (phone != null && phone.Length > 0)
                url = String.Concat(url, "&telefonoEmpleado=", HttpUtility.UrlEncode(phone));

            var response = await MakeApiKeyHttpCall<BaseResponse<UserPaperData>, string>(url, HttpVerbMethod.Get, null)
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
            return SecurityMapper.MapUser(response.data.ElementAt(0));
        }

        public async Task GenerateManual(int idEmpleado, bool isGreenPaper, DateTime dateTime)
        {
            string url = String.Format(String.Concat(DataConstants.Endpoint, DataConstants.GenerationManualURL), idEmpleado);

            var request = new GenerationManualRequest()
            {
                idEmployee = idEmpleado,                
                isGreenPaper = isGreenPaper,
                registrationDateTime = dateTime.ToString(EmployeeMapper.DATEFORMAT)
            };

            var response = await MakeApiKeyHttpCall<BaseResponse, GenerationManualRequest>(url, HttpVerbMethod.Post, request)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info.Select(x => x.message));
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }
        }

        public async Task<IEnumerable<Location>> GetLocations()
        {
            string url = String.Concat(DataConstants.Endpoint, DataConstants.SecurityLocationsURL);
            
            var response = await MakeApiKeyHttpCall<BaseResponse<LocationData>, string>(url, HttpVerbMethod.Get, null)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info.Select(x => x.message));
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }
            return SecurityMapper.MapLocationList(response.data);
        }
    }
}
