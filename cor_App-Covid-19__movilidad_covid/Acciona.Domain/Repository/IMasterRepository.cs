using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Acciona.Domain.Model.Master;
using Acciona.Domain.Model.Security;

namespace Acciona.Domain.Repository
{
    public interface IMasterRepository
    {
        Task<IEnumerable<PassportState>> GetPassportStates();
        Task<IEnumerable<PassportStateColor>> GetPassportStatesColors();        
        Task<IEnumerable<MedicalMonitor>> GetMedicalMonitors();
        Task<IEnumerable<RiskFactor>> GetRiskFactors();
        Task<IEnumerable<SymptomType>> GetSymtomTypes();
        Task<IEnumerable<Location>> GetLocations();
    }
}
