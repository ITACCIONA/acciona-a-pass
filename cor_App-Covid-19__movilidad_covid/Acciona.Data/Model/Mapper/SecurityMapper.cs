using Acciona.Data.Model.Security;
using Acciona.Domain.Model.Security;
using Acciona.Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acciona.Data.Model.Mapper
{
    public class SecurityMapper : BaseMapper
    {
        public static UserPaper MapUser(UserPaperData data)
        {
            var user = new UserPaper()
            {
                IdEmpleado=data.idEmpleado,
                NombreEmpleado=data.nombreEmpleado,
                ApellidosEmpleado=data.apellidosEmpleado,
                InicialesEmpleado=data.inicialesEmpleado,
                DNI=data.dni,
                EdadEmpleado=data.edadEmpleado,
                MailEmpleado=data.mailEmpleado,
                NumEmpleado=data.numEmpleado,
                TelefonoEmpleado=data.telefonoEmpleado
            };            
            return user;
        }

        public static Location MapLocation(LocationData data)
        {
            return new Location()
            {
                IdLocation=data.idLocation,
                Name=data.name,
                Ciudad=data.ciudad,
                CodPostal=data.codPostasl,
                Direccion=data.direccion,
                Pais=data.pais,
            };            
        }

        public static IEnumerable<Location> MapLocationList(IEnumerable<LocationData> dataList)
        {
            return dataList?.Select(a => MapLocation(a));
        }
    }

}
