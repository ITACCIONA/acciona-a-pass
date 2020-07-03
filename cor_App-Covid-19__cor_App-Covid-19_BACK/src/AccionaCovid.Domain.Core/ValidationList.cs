using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AccionaCovid.Domain.Core
{
    /// <summary>
    /// Contains All the validations that an Entity needs
    /// </summary>
    public class ValidationList<T> : IEnumerable<EntityValidation<T>> where T : Entity<T>
    {
        /// <summary>
        /// Inner list to hold the validations
        /// </summary>
        private readonly List<EntityValidation<T>> innerList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ValidationList()
        {
            this.innerList = new List<EntityValidation<T>>();
        }

        /// <summary>
        /// Constructor with capacity
        /// </summary>
        /// <param name="capacity"></param>
        public ValidationList(int capacity)
        {
            this.innerList = new List<EntityValidation<T>>(capacity);
        }

        /// <summary>
        /// Adds a new entity validation
        /// </summary>
        /// <param name="code"></param>
        /// <param name="validation"></param>
        /// <param name="parameters"></param>
        public void Add(String code, Expression<Func<T, bool>> validation, params object[] parameters)
            => innerList.Add(new EntityValidation<T>(code, validation, parameters));

        /// <summary>
        /// Implements the IEnumerable interface
        /// </summary>
        /// <returns></returns>
        public IEnumerator<EntityValidation<T>> GetEnumerator() => innerList.GetEnumerator();

        /// <summary>
        /// Implements the IEnumerable interface
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => innerList.GetEnumerator();
    }
}
