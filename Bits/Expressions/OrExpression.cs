using System.Collections.Generic;
using System.Linq;

namespace Bits.Expressions
{
    public class OrExpression : BranchExpression
    {
        public OrExpression(params Expression[] expressions)
            : base(Operator.Or, expressions)
        {
        }

        public override Expression Simplify()
        {
            var expressions = new HashSet<Expression>(ExpressionComparer.Default);

            foreach (var expression in Expressions)
            {
                if (expression == ValueExpression.True)
                    return ValueExpression.True;
                if (expression != ValueExpression.False)
                    expressions.Add(expression);
            }
            if (expressions.Count == 0)
                return ValueExpression.False;
            if (expressions.Count == 1)
                return expressions.First();

            return Simplify(new OrExpression(expressions.ToArray()));
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}