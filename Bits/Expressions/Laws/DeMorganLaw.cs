using System.Linq;

namespace Bits.Expressions.Laws
{
    public class DeMorganLaw : ExpressionVisitor
    {
        public override Expression Visit(NotExpression notExpression)
        {
            var operand = notExpression.Operand as ComplexExpression;
            if (operand != null)
            {
                return new ComplexExpression(operand.Operator.Dual(), operand.Select(Expression.Not));
            }
            return notExpression;
        }
    }
}