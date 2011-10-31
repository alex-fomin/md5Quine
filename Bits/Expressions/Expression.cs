using System.Linq;

namespace Bits.Expressions
{
    public abstract class Expression
    {
        public virtual Expression Simplify()
        {
            return Simplify(this);
        }



        public abstract T Accept<T>(IExpressionVisitor<T> visitor);

        public static Expression Not(Expression expression)
        {
            return new NotExpression(expression).Simplify();
        }

        public static Expression And(params Expression[] expressions)
        {
            return new AndExpression(expressions).Simplify();
        }

        public static Expression Or(params Expression[] expressions)
        {
            return new OrExpression(expressions).Simplify();
        }

        protected static Expression Simplify(Expression expression)
        {
            
            expression = DeMorgan(expression);
            expression = Taut(expression);
            expression = Absorbtion(expression);
            return expression;
        }

        private static Expression Absorbtion(Expression expression)
        {
            // a & (a | b) = a
            // a | (a & b) = a
            var branch = expression as BranchExpression;
            if (branch != null)
            {
                
            }
            return expression;
        }

        private static Expression Taut(Expression expression)
        {
            // remove a * ~a and replace it with True or False

            var branchExpression = expression as BranchExpression;
            if (branchExpression != null)
            {
                if (branchExpression.Expressions.OfType<NotExpression>().Any(notExpression => branchExpression.Expressions.Contains(notExpression.Operand)))
                {
                    return branchExpression.Operator == Operator.And
                               ? ValueExpression.False
                               : ValueExpression.True;
                }
            }
            return expression;
        }

        private static Expression DeMorgan(Expression expression)
        {
            var notExpression = expression as NotExpression;
            if (notExpression != null)
            {
                var branchExpression = notExpression.Operand as BranchExpression;
                if (branchExpression != null)
                {
                    var negate = branchExpression.Expressions.Select(Not);
                    return branchExpression.Operator == Operator.And
                               ? (Expression) new OrExpression(negate.ToArray())
                               : new AndExpression(negate.ToArray());
                }
            }
            return expression;
        }
    }

    public interface IExpressionVisitor<T>
    {
        T Visit(NotExpression notExpression);
        T Visit(VariableExpression variableExpression);
        T Visit(ValueExpression valueExpression);
        T Visit(AndExpression andExpression);
        T Visit(OrExpression andExpression);
    }
}