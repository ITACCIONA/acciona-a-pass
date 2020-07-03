using System;
using System.Collections.Generic;
using System.Linq;

namespace AccionaCovid.Application.Core
{
    /// <summary>
    /// Clase que implementa listas paginadas a partir de una lista genérica.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> where T : class
    {
        /// <summary>
        /// Constructor por defecto. Solo se permite usar para heredar.
        /// </summary>
        public PagedList() { }

        /// <summary>
        /// Crea un PagedList a partir de un conjunto de datos de memoria. 
        /// Es el método a usar si no hay paginación.
        /// </summary>
        /// <param name="data"></param>
        public PagedList(List<T> data)
        {
            Page = 1;
            InternalList = data;
            NumElements = data.Count;
            ElementsPerPage = data.Count;
        }

        /// <summary>
        /// Crea un paged list usando los valores proporcionados. La query se ejecuta dos veces.
        /// </summary>
        /// <param name="pageNumber">Número de página.</param>
        /// <param name="elementsPerPage">Elementos en cada página.</param>
        /// <param name="query">Query a ejecutar.</param>
        public PagedList(int pageNumber, int elementsPerPage, IEnumerable<T> query)
        {
            if (pageNumber <= 0) throw new ArgumentException(@"must be greater than 0", nameof(pageNumber));

            if (elementsPerPage <= 0) throw new ArgumentException(@"must be greater than 0", nameof(elementsPerPage));

            Page = pageNumber;
            ElementsPerPage = elementsPerPage;

            if (query == null) return;

            var enumerable = query as IList<T> ?? query.ToList();

            NumElements = enumerable.Count;
            InternalList = enumerable.Skip((pageNumber - 1) * elementsPerPage).Take(elementsPerPage).ToList();
        }

        /// <summary>
        /// Lista interna de objetos de tipo T.
        /// </summary>
        public List<T> InternalList { get; protected set; }

        /// <summary>
        /// Número de página.
        /// </summary>
        public int Page { get; protected set; }

        /// <summary>
        /// Elementos totales.
        /// </summary>
        public int NumElements { get; protected set; }

        /// <summary>
        /// Número de elementos por página.
        /// </summary>
        public int ElementsPerPage { get; protected set; }

        /// <summary>
        /// Número de páginas totales.
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (ElementsPerPage == 0) return 1;

                var totalPages = NumElements / ElementsPerPage;

                if (NumElements % ElementsPerPage > 0)
                    totalPages++;

                return totalPages;

            }
        }
    }
}
