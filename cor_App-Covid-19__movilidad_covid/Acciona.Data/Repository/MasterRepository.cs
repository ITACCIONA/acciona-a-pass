using Acciona.Data.Model;
using Acciona.Data.Model.Admin;
using Acciona.Data.Model.Mapper;
using Acciona.Data.Model.Master;
using Acciona.Data.Model.Security;
using Acciona.Data.Repository.Base;
using Acciona.Domain.Model.Admin;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Model.Master;
using Acciona.Domain.Model.Security;
using Acciona.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Data.Repository
{
    public class MasterRepository : ApiBaseService, IMasterRepository
    {
        public async Task<IEnumerable<PassportState>> GetPassportStates()
        {
            string url = String.Concat(DataConstants.Endpoint,DataConstants.PassportStatesURL);

            var response = await MakeSessionHttpCall<BaseResponse<PassportStateData>, string>(url, HttpVerbMethod.Get, null)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info);
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }

            return MasterMapper.MapPassportStateList(response.data);
        }

        public async Task<IEnumerable<PassportStateColor>> GetPassportStatesColors()
        {
            string url = String.Concat(DataConstants.Endpoint, DataConstants.PassportStatesColorsURL);

            var response = await MakeSessionHttpCall<BaseResponse<PassportStateColorData>, string>(url, HttpVerbMethod.Get, null)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info);
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }

            return MasterMapper.MapPassportStateColorList(response.data);
        }


        public async Task<IEnumerable<MedicalMonitor>> GetMedicalMonitors()
        {
            string url = String.Concat(DataConstants.Endpoint, DataConstants.MedicalMonitoringURL);

            var response = await MakeSessionHttpCall<BaseResponse<MedicalMonitorData>, string>(url, HttpVerbMethod.Get, null)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info);
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }

            return MasterMapper.MapMedicalMonitorList(response.data);
        }

        public async Task<IEnumerable<RiskFactor>> GetRiskFactors()
        {
            string url = String.Concat(DataConstants.Endpoint, DataConstants.RiskFactorsURL);

            var response = await MakeSessionHttpCall<BaseResponse<RiskFactorData>, string>(url, HttpVerbMethod.Get, null)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info);
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }

            return MasterMapper.MapRiskFactorList(response.data);
        }

        public async Task<IEnumerable<SymptomType>> GetSymtomTypes()
        {
            string url = String.Concat(DataConstants.Endpoint, DataConstants.SymptomTypesURL);

            var response = await MakeSessionHttpCall<BaseResponse<SymptomTypeData>, string>(url, HttpVerbMethod.Get, null)
                .ConfigureAwait(false);
            if (response.HasError)
            {
                if (response.info != null && response.info.Count() > 0)
                    response.Message = String.Join("\n", response.info);
                throw new ApiException()
                {
                    Code = response.status,
                    Error = response.Message
                };
            }

            return MasterMapper.MapSymptomTypesList(response.data);
        }

        public async Task<IEnumerable<Location>> GetLocations()
        {
            string url = String.Concat(DataConstants.Endpoint, DataConstants.LocationsURL);

            var response = await MakeSessionHttpCall<BaseResponse<LocationData>, string>(url, HttpVerbMethod.Get, null)
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
