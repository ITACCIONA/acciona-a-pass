using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AccionaCovid.Crosscutting
{
    /// <summary>
    /// This class is used to replace parameters of a Expression
    /// https://blogs.msdn.microsoft.com/meek/2008/05/02/linq-to-entities-combining-predicates/
    /// </summary>
    public class ParameterRebinder : ExpressionVisitor
    {
        /// <summary>
        /// Utility facade method
        /// </summary>
        /// <param name="map">A map with the parameters to replace</param>
        /// <param name="exp">The expression where you want to replace</param>
        /// <returns>The replaced Expression</returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        /// <summary>
        /// Parameters to replace
        /// </summary>
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="map"></param>
        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        /// Takes the new parameters, if any, to replace it
        /// </summary>
        /// <param name="p">The parameter to replace</param>
        /// <returns>The new, replaced Expression</returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;

            if (map.TryGetValue(p, out replacement))
                p = replacement;

            return base.VisitParameter(p);
        }
    }
}
