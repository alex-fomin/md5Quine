namespace Bits.Expressions.Laws
{
    public class ComplementationLaw : ExpressionVisitor
    {
        public override Expression Visit(ComplexExpression complex)
        {
            for (int i = 0; i < complex.Expressions.Count; i++)
            {
                var left = complex.Expressions[i] as NotExpression;
                if (left != null)
                {
                    for (int j = 0; j < i; j++)
                    {
                        var right = complex.Expressions[j];
                        if (left.Operand == right)
                        {
                            return complex.Operator == Operator.And ? Expression.False : Expression.True;
                        }
                    }
                }
            }
            return complex;
        }
    }
}