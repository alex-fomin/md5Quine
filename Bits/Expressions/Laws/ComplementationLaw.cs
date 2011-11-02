namespace Bits.Expressions.Laws
{
    public class ComplementationLaw : ExpressionVisitor
    {
        public override Expression Visit(ComplexExpression complex)
        {
            for (int i = 0; i < complex.Expressions.Count; i++)
            {
                var left =complex.Expressions[i];

                for (int j = i + 1; j < complex.Expressions.Count; j++)
                {
                    var right = complex.Expressions[j];
                    
                    if ((left is NotExpression && ((NotExpression)left).Operand == right )||
                        (right is NotExpression && ((NotExpression)right).Operand == left))
                    {
                        return complex.Operator == Operator.And ? Expression.False : Expression.True;
                    }

                }
            }
            return complex;
        }
    }
}