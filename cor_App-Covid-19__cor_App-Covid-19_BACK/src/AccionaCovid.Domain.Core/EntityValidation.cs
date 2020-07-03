using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AccionaCovid.Domain.Core
{
    /// <summary>
    /// Represents a validation of an Entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityValidation<T>
    {
        /// <summary>
        /// The error code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Validation to make
        /// </summary>
        public Expression<Func<T, bool>> Validation { get; set; }

        /// <summary>
        /// Parameters for the error message
        /// </summary>
        public object[] MessageParameters { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="validation"></param>
        /// <param name="messageParameters"></param>
        public EntityValidation(string code, Expression<Func<T, bool>> validation, object[] messageParameters)
        {
            Code = code;
            Validation = validation;
            MessageParameters = messageParameters;
        }
    }
}
