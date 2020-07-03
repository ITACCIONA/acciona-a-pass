using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Data.Model
{
    public class BaseData<T>
    {
        public int page { get; set; }
        public int numElements { get; set; }
        public int elementsPerPage { get; set; }
        public int totalPages { get; set; }
        public IEnumerable<T> internalList { get; set; }
    }
}
