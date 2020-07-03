using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Data
{
    public static class DataConstants
    {
        //URL         
        public const string Endpoint = "https://foo-webapi.bar"; //Pro   "https://foo-webapi.bar"; //Dev  
        public const string EndpointVersions = "https://foo-blob.bar"; //Pro  "https://foo-blob.bar"; //Dev  
        public const string XapiKey = "XapiKeySecret"; //Pro    "XapiKeySecret"; //Dev  
        public const bool SendAnalytics = false;
        

        //Versions
        public const string VersionsURL = "/webestados/versions.json";
        public const string ResourcesURL = "/webestados/Estado_{0}_{1}.htm";

        //Admin
        public const string UserURL = "/api/Admin/login/employee";

        //Employee
        public const string AlertsURL = "/api/Employees/Self/alerts";
        public const string AlertReadURL = "/api/Employees/Self/alert/{0}";
        public const string PassportURL = "/api/Employees/Self/Passport";
        public const string FichaURL = "/api/Employees/Self/ficha";
        public const string ModifyFichaURL = "/api/Employees/Self/ficha";
        public const string SendSymptomsURL = "/api/Employees/Self/Symptoms";
        public const string SendRiskFactorURL = "/api/Employees/Self/riskFactor";
        public const string PanicURL = "/api/Employees/Self/panic?currentDeviceDateTime={0}";

        //Master
        public const string PassportStatesURL = "/api/Master/PassportStates";
        public const string PassportStatesColorsURL = "/api/Master/PassportStatesColors";
        public const string PassportActionsURL = "/api/Master/PassportActions";
        public const string MedicalMonitoringURL = "/api/Master/MedicalMonitoring";
        public const string RiskFactorsURL = "/api/Master/RiskFactors";
        public const string SymptomTypesURL = "/api/Master/SymptomTypes";
        public const string LocationsURL = "/api/Master/locations";

        //Seguridad
        public const string SendTemperatureURL = "/api/SecurityScan/Employees/{0}/TemperatureMedition";
        public const string SecurityPassportURL = "/api/SecurityScan/employee/{0}/Passports/Active";
        public const string SecurityManualURL = "/api/SecurityScan/employee?dniEmpleado={0}";
        public const string GenerationManualURL = "/api/SecurityScan/Employees/{0}/GenerationManual";
        public const string SecurityLocationsURL = "/api/SecurityScan/locations";
        //API
        public static TimeSpan ApiTimeout = TimeSpan.FromSeconds(15);

        
    }
}
