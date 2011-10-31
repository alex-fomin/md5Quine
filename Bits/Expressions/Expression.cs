using System;

namespace Bits.Expressions
{
    public abstract class Expression : IEquatable<Expression>
    {
        public static VariableExpression Variable(string name)
        {
            return new VariableExpression(name);
        }

        public static NotExpression Not(Expression exp)
        {
            return new NotExpression(exp);
        }

        public static Expression Constant(bool b)
        {
            return new ConstantExpression(b);
        }

        public static Expression And(Expression left, Expression right)
        {
            return new BinaryExpression(left, right, ExpressionType.And);
        }

        public static BinaryExpression Or(Expression left, Expression right)
        {
            return new BinaryExpression(left, right, ExpressionType.Or);
        }

        public abstract bool Equals(Expression other);

        public override bool Equals(object obj)
        {
            return Equals((Expression) obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public static bool operator ==(Expression left, Expression right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Expression left, Expression right)
        {
            return !Equals(left, right);
        }
    }


    public enum ExpressionType
    {
        And, Or, Xor
    }


}