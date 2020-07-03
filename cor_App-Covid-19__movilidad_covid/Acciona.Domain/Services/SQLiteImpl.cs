using SQLite;
using System.IO;

namespace Acciona.Domain.Services
{
    public class SQLiteImpl : ISQLite
    {
        private string GetPath()
        {
            var dbName = "AccionaIndustria.db3";
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
            return path;
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(GetPath());
        }
    }
}
