using System;
using System.Linq;

namespace Bits.Expressions
{
    public abstract class Expression : IEquatable<Expression>
    {
    	public static readonly ValueExpression False = new ValueExpression(false);
    	public static readonly Expression True = new ValueExpression(true);

        public static VariableExpression Variable(string name)
        {
            return new VariableExpression(name);
        }

        #region Operator

        public static Expression operator &(Expression a, Expression b)
		{
			return And(a, b);
		}
		public static Expression operator |(Expression a, Expression b)
		{
			return Or(a, b);
		}
		public static Expression operator ^(Expression a, Expression b)
		{
		    return ((a & ~b) | (~a & b));
		}

		public static Expression operator ~(Expression a)
		{
			return Not(a);
		}

        #endregion


        public abstract T Accept<T>(IExpressionVisitor<T> visitor);

        public static Expression Not(Expression expression)
        {
            return new NotExpression(expression);
        }

        public static Expression And(params Expression[] expressions)
        {
            return new ComplexExpression(Operator.And, expressions);
        }

        public static Expression Or(params Expression[] expressions)
        {
            return new ComplexExpression(Operator.Or, expressions);
        }

        public bool Equals(Expression other)
        {
            return ExpressionComparer.Default.Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals((Expression) obj);
        }

        public override int GetHashCode()
        {
            return ExpressionComparer.Default.GetHashCode(this);
        }


        public static bool operator ==(Expression left, Expression right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Expression left, Expression right)
        {
            return !Equals(left, right);
        }

    	public Expression Simlpify()
    	{
    		return this.Accept(Simplifier.Instance);
    	}
    }
}