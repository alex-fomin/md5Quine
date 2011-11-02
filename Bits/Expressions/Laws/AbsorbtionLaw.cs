using System.Collections.Generic;

namespace Bits.Expressions.Laws
{
    public class AbsorbtionLaw : ExpressionVisitor
    {
        public override Expression Visit(ComplexExpression complex)
        {
            var expressions = new List<Expression>();
            int count = complex.Expressions.Count;
            for (int i = 0; i < count; i++)
            {
                bool found = false;
                var left = complex.Expressions[i];
                for (int j=i+1;j<count;j++)
                {
                    var right = complex.Expressions[j];
                    if (left == right)
                    {
                        found = true;
                    }
                }
                if (!found)
                    expressions.Add(left);

            }

            if (expressions.Count == 1)
                return expressions[0];
            return expressions.Count != count ? new ComplexExpression(complex.Operator, expressions) : complex;
        }
    }
}