using Acciona.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Domain.Repository
{
    public interface ISQLiteRepository
    {
        void AddItem<T>(T item);
        void AddAll<T>(IEnumerable<T> items);
        void UpdateItem<T>(T item);
        void DeleteItem<T>(T item);
        void DeleteAll<T>();
        IEnumerable<Sincro> GetPendingSincro();
        void AddSincro(IEnumerable<Sincro> sincroData);
    }
}
