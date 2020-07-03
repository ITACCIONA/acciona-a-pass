using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Domain.Services
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
