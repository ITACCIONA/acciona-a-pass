using Acciona.Domain.Model.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Domain.Repository
{
    public interface IAdminRepository
    {
        Task<User> GetUser();
    }
}
