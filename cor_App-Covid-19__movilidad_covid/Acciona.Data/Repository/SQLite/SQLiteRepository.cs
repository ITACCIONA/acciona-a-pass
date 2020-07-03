using Acciona.Data.Repository.Base;
using Acciona.Domain.Model;
using Acciona.Domain.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acciona.Data.Repository.SQLite
{
    public class SQLiteRepository: BaseSQLite, ISQLiteRepository
    {
        public SQLiteRepository()
        {
            db.CreateTable<Sincro>();
            
        }

        public void AddItem<T>(T item)
        {
            db.BeginTransaction();
            db.Insert(item);
            db.Commit();
        }

        public void AddAll<T>(IEnumerable<T> items)
        {
            db.BeginTransaction();
            db.InsertAll(items);
            db.Commit();
        }

        public void DeleteItem<T>(T item)
        {
            db.BeginTransaction();
            db.Delete(item);
            db.Commit();
        }

        public void DeleteAll<T>()
        {
            db.BeginTransaction();
            db.DeleteAll<T>();
            db.Commit();
        }

        public void UpdateItem<T>(T item)
        {
            db.BeginTransaction();
            var updated=db.InsertOrReplace(item);
            db.Commit();
        }
        

        public IEnumerable<Sincro> GetPendingSincro()
        {
            db.BeginTransaction();
            var list = db.Table<Sincro>();
            db.Commit();
            return list;
        }


        public void AddSincro(IEnumerable<Sincro> sincroData)
        {
            foreach (var s in sincroData)
                AddItem<Sincro>(s);
        }

    }
}
