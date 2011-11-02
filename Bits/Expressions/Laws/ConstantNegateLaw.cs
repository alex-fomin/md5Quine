namespace Bits.Expressions.Laws
{
    public class ConstantNegateLaw : ExpressionVisitor
    {
        public override Expression Visit(NotExpression notExpression)
        {
            if (notExpression.Operand == Expression.True)
                return Expression.False;
            if (notExpression.Operand == Expression.False)
                return Expression.True;

            return notExpression;
        }
    }
}