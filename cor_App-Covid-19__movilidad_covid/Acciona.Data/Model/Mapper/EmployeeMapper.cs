using Acciona.Data.Model.Admin;
using Acciona.Data.Model.Employee;
using Acciona.Domain.Model.Admin;
using Acciona.Domain.Model.Base;
using Acciona.Domain.Model.Employee;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Acciona.Data.Model.Mapper
{
    public class EmployeeMapper : BaseMapper
    {
        public const string DATEFORMAT = "yyyy-MM-dd'T'HH:mm:ssK";

        public static IEnumerable<Alert> MapAlertsResponse(AlertsResponse data)
        {
            List<Alert> alerts = new List<Alert>();
            foreach (var a in data.alertsNoRead)
            {
                alerts.Add(MapAlert(a, false));
            }
            foreach (var a in data.alertsRead)
            {
                alerts.Add(MapAlert(a, true));
            }
            return alerts;
        }

        public static Alert MapAlert(AlertData data, bool read)
        {
            return new Alert()
            {
                IdAlert = data.idAlert,
                Title = data.title,
                Comment = data.comment,
                FechaNotificacion = DateTime.ParseExact(data.fechaNotificacion, DATEFORMAT, CultureInfo.InvariantCulture),
                Read = read
            };
        }

        public static Passport MapPassport(PassportData data)
        {
            return new Passport()
            {
                IdEmpleado = data.idEmpleado,
                IdPassport = data.idPassport,
                NombreEmpleado = data.nombreEmpleado,
                InicialesEmpleado = data.inicialesEmpleado,
                EdadEmpleado = data.edadEmpleado,
                NumEmpleado = data.numEmpleado,
                Departamento = data.departamento,
                NameLocalizacion = data.nameLocalizacion,
                Pais = data.pais,
                Direccion1 = data.direccion1,
                Ciudad = data.ciudad,
                CodigoPostal = data.codigoPostal,
                Division = data.division,
                EstadoPasaporte = data.estadoPasaporte,
                ColorPasaporte=data.colorPasaporte,                
                NumTest = data.numTest,
                HasMessage=data.hasMessage,
                EstadoId=data.estadoId,
                FechaCreacion = data.fechaCreacion != null ? DateTime.ParseExact(data.fechaCreacion, DATEFORMAT, CultureInfo.InvariantCulture) : (DateTime?)null,
                FechaExpiracion = data.fechaExpiracion != null ? DateTime.ParseExact(data.fechaExpiracion, DATEFORMAT, CultureInfo.InvariantCulture) : (DateTime?)null
            };
        }

        public static RiskFactorValue MapRiskFactorValue(RiskFactorValueData data)
        {
            return new RiskFactorValue()
            {
                IdRiskFactor = data.idRiskFactor,
                Name = data.name,
                Value = data.value,
                FechaFactor = DateTime.ParseExact(data.fechaFactor, DATEFORMAT, CultureInfo.InvariantCulture)
            };
        }

        public static IEnumerable<RiskFactorValue> MapRiskFactorValueList(IEnumerable<RiskFactorValueData> dataList)
        {
            return dataList?.Select(a => MapRiskFactorValue(a));
        }

        public static Ficha MapFicha(FichaData data)
        {
            return new Ficha()
            {
                IdEmpleado = data.idEmpleado,
                NombreEmpleado = data.nombreEmpleado,
                ApellidosEmpleado = data.apellidosEmpleado,
                InicialesEmpleado = data.inicialesEmpleado,
                DNI = data.dni,
                EdadEmpleado = data.edadEmpleado,
                NumEmpleado = data.numEmpleado,
                TelefonoEmpleado = data.telefonoEmpleado,
                MailEmpleado = data.mailEmpleado,
                ValoracionFactorRiesgos = MapRiskFactorValueList(data.valoracionFactorRiesgos),
                IdLocalizacion=data.idLocalizacion,
                Localizacion=data.localizacion
            };
        }

        public static RequestValueData MapRequestValueData(RequestValue data)
        {
            return new RequestValueData()
            {
                id=data.Id,
                value=data.Value
            };
        }

        public static IEnumerable<RequestValueData> MapRequestValueDataList(IEnumerable<RequestValue> dataList)
        {
            return dataList?.Select(a => MapRequestValueData(a));
        }

        public static RequestRiskFactorValueData MapRequestRiskFactorValueData(RequestValue data)
        {
            return new RequestRiskFactorValueData()
            {
                idRiskFactor = data.Id,
                value = data.Value
            };
        }

        public static IEnumerable<RequestRiskFactorValueData> MapRequestRiskFactorValueDataList(IEnumerable<RequestValue> dataList)
        {
            return dataList?.Select(a => MapRequestRiskFactorValueData(a));
        }
    }
}
