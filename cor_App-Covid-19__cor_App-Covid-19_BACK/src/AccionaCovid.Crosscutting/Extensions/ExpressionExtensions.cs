using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AccionaCovid.Crosscutting
{
    /// <summary>
    /// This class contains some utility methods to work with Expression composition.
    /// </summary>
    public static class ExpressionExtensions
    {
        public static Expression<T> Compose<T>(this Expression<T> left, Expression<T> right, Func<Expression, Expression, Expression> merge)
        {
            /* Build parameter map (from parameters of second to parameters of first) */
            Dictionary<ParameterExpression, ParameterExpression> map = left.Parameters
                .Select((first, index) => new { first, second = right.Parameters[index] })
                .ToDictionary(p => p.second, p => p.first);

            /* Replace parameters in the second lambda expression with parameters from the first */
            Expression secondBody = ParameterRebinder.ReplaceParameters(map, right.Body);

            /* apply composition of lambda expression bodies to parameters from the first expression */
            return Expression.Lambda<T>(merge(left.Body, secondBody), left.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }
    }
}
