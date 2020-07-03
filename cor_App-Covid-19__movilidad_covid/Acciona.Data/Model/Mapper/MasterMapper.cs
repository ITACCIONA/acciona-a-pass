using Acciona.Data.Model.Master;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Model.Master;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acciona.Data.Model.Mapper
{
    public class MasterMapper : BaseMapper
    {
        public static PassportState MapPassportState(PassportStateData data)
        {
            return new PassportState()
            {
                Id=data.id,
                Name=data.name
            };            
        }

        public static IEnumerable<PassportState> MapPassportStateList(IEnumerable<PassportStateData> dataList)
        {
            return dataList?.Select(a => MapPassportState(a));
        }

        public static PassportStateColor MapPassportStateColor(PassportStateColorData data)
        {
            return new PassportStateColor()
            {
                Id = data.id,
                Name = data.name
            };
        }

        public static IEnumerable<PassportStateColor> MapPassportStateColorList(IEnumerable<PassportStateColorData> dataList)
        {
            return dataList?.Select(a => MapPassportStateColor(a));
        }
       

        public static Parameter MapParameter(ParameterData data)
        {
            return new Parameter()
            {
                IdParameter = data.idParameter,
                Name = data.name
            };
        }

        public static IEnumerable<Parameter> MapParameterList(IEnumerable<ParameterData> dataList)
        {
            return dataList?.Select(a => MapParameter(a));
        }

        public static MedicalMonitor MapMedicalMonitor(MedicalMonitorData data)
        {
            return new MedicalMonitor()
            {
                IdParameterType = data.idParameterType,
                Name = data.name,
                Parameters = MapParameterList(data.parameters)
            };
        }

        public static IEnumerable<MedicalMonitor> MapMedicalMonitorList(IEnumerable<MedicalMonitorData> dataList)
        {
            return dataList?.Select(a => MapMedicalMonitor(a));
        }

        public static RiskFactor MapRiskFactor(RiskFactorData data)
        {
            return new RiskFactor()
            {
                IdRiskFactor = data.idRiskFactor,
                Name = data.name
            };
        }

        public static IEnumerable<RiskFactor> MapRiskFactorList(IEnumerable<RiskFactorData> dataList)
        {
            return dataList?.Select(a => MapRiskFactor(a));
        }

        public static SymptomType MapSymptomType(SymptomTypeData data)
        {
            return new SymptomType()
            {
                IdSymptomTypes = data.idSymptomTypes,
                Name = data.name
            };
        }

        public static IEnumerable<SymptomType> MapSymptomTypesList(IEnumerable<SymptomTypeData> dataList)
        {
            return dataList?.Select(a => MapSymptomType(a));
        }
    }
}
