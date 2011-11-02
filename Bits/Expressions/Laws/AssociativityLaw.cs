using System.Collections.Generic;

namespace Bits.Expressions.Laws
{
    public class AssociativityLaw : ExpressionVisitor
    {
        public override Expression Visit(ComplexExpression complex)
        {
            bool modified = false;
            var expressions = new List<Expression>();
            foreach (var expression in complex.Expressions)
            {
                var nestedComplex = expression as ComplexExpression;
                if (nestedComplex != null && nestedComplex.Operator == complex.Operator)
                {
                    expressions.AddRange(nestedComplex.Expressions);
                    modified = true;
                }
                else
                {
                    expressions.Add(expression);
                }
            }
            return modified ? new ComplexExpression(complex.Operator, expressions) : complex;
        }
    }
}