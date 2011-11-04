using System.Collections.Generic;
using System.Linq;

namespace Bits.Expressions.Laws
{
    public class IdentityAnnihilationLaw : ExpressionVisitor
    {
        public override Expression Visit(ComplexExpression complex)
        {
            var expressions = new List<Expression>();
            if (complex.Operator == Operator.And)
            {
                foreach (var expression in complex)
                {
                    if (expression == Expression.False)
                        return Expression.False;
                    if (expression != Expression.True)
                        expressions.Add(expression);
                }
            }
            else
            {
                foreach (var expression in complex)
                {
                    if (expression == Expression.True)
                        return Expression.True;
                    if (expression != Expression.False)
                        expressions.Add(expression);
                }
            }
			if (expressions.Count == 0)
				return complex.Operator == Operator.And ? Expression.True : Expression.False;

            if (expressions.Count == 1)
                return expressions[0];
            if (expressions.Count != complex.Count)
            {
                return new ComplexExpression(complex.Operator, expressions);
            }
            return complex;
        }
    }
}