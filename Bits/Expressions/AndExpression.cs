using System.Collections.Generic;
using System.Linq;

namespace Bits.Expressions
{
    public class AndExpression : BranchExpression
    {
        public AndExpression(IEnumerable<Expression> expressions)
            : base(Operator.And, expressions)
        {
        }

        public override Expression Simplify()
        {
            var expressions = new HashSet<Expression>(ExpressionComparer.Default);

            foreach (var expression in Expressions)
            {
                if (expression == ValueExpression.False)
                    return ValueExpression.False;
                if (expression != ValueExpression.True)
                    expressions.Add(expression);
            }
            if (expressions.Count == 0)
                return ValueExpression.True;

            if (expressions.Count == 1)
                return expressions.First();

            return Simplify(new AndExpression(expressions));

        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}