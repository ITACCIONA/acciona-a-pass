using Acciona.Data.Model.Admin;
using Acciona.Domain.Model.Admin;
using Acciona.Domain.Model.Base;
using System;

namespace Acciona.Data.Model.Mapper
{
    public class AdminMapper : BaseMapper
    {
        public static User MapUser(UserData data)
        {
            var user = new User()
            {
                IdEmpleado=data.idEmpleado,
                NombreUsuario=data.nombreUsuario,
                InicialesUsuario=data.inicialesUsuario,
            };            
            return user;
        }
    }
}
