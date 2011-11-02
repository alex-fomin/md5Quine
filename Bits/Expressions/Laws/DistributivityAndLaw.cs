using System.Collections.Generic;
using System.Linq;

namespace Bits.Expressions.Laws
{
    public class DistributivityAndLaw : ExpressionVisitor
    {
        public override Expression Visit(ComplexExpression complex)
        {
            if (complex.Operator == Operator.And)
            {
                var andExpressions = new List<Expression>();
                var orExpressions = new List<ComplexExpression>();

                foreach (var expression in complex.Expressions)
                {
                    var inner = expression as ComplexExpression;
                    if (inner != null && inner.Operator == Operator.Or)
                    {
                        orExpressions.Add(inner);
                    }
                    else
                    {
                        andExpressions.Add(expression);
                    }
                }



                if (orExpressions.Count == 0)
                    return complex;

                var product = orExpressions.Select(x => x.Expressions).CartesianProduct();

                var result =
                    product.Select(p => new ComplexExpression(Operator.And, p.Concat(andExpressions)));

                return new ComplexExpression(Operator.Or, result);

            }
            return complex;
        }
    }
}