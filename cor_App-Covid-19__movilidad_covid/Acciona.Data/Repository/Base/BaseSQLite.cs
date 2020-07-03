using Acciona.Domain.Services;
using ServiceLocator;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Data.Repository.Base
{
    public class BaseSQLite
    {

        protected ISQLite iSQLite;
        protected SQLiteConnection db;


        protected BaseSQLite()
        {
            iSQLite = Locator.Current.GetService<ISQLite>();
            db = iSQLite.GetConnection();
        }
    }
}
