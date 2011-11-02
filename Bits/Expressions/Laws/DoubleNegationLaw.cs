namespace Bits.Expressions.Laws
{
    public class DoubleNegationLaw : ExpressionVisitor
    {
        public override Expression Visit(NotExpression notExpression)
        {
            var operand = notExpression.Operand as NotExpression;
            return operand != null ? operand.Operand : notExpression;
        }
    }
}