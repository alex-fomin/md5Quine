using System.Collections.Generic;

namespace Bits.Expressions
{
    public class ExpressionRewriter : ExpressionVisitor
    {
        public override Expression Visit(NotExpression notExpression)
        {
            var expression = notExpression.Operand.Accept(this);
            return expression != notExpression ? Expression.Not(expression) : notExpression;
        }

        public override Expression Visit(ComplexExpression complex)
        {
            bool modified = false;
            var expressions = new List<Expression>();
            foreach (var expression in complex)
            {
                var visited = expression.Accept(this);
                expressions.Add(visited);
                if (visited != expression)
                    modified = true;
            }
            return modified ? new ComplexExpression(complex.Operator, expressions) : complex;
        }
    }
}