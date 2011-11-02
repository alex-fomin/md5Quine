using System;
using Bits.Expressions;

namespace Bits
{
    public class Bit
    {
        private readonly Expression _expression;
        public static readonly Bit False = new Bit(ValueExpression.False);
        public static readonly Bit True = new Bit(ValueExpression.True);

        public Bit(string name)
            : this(new VariableExpression(name))
        {
        }

        private Bit(Expression expression)
        {
            _expression = expression;
        }

        public static Bit operator ~(Bit a)
        {
            return new Bit(Expression.Not(a._expression));
        }

        public static Bit operator &(Bit a, Bit b)
        {
            return new Bit(Expression.And(a._expression,b._expression));
        }

        public static Bit operator |(Bit a, Bit b)
        {
            return new Bit(Expression.Or(a._expression, b._expression));
        }

        public static Bit operator ^(Bit a, Bit b)
        {
            //return new Bit(Expression.ExclusiveOr(a._exp, b._exp));
            return ((~a) & (b) | (a & (~b)));
        }

        public override string ToString()
        {
            return _expression.ToString();
        }

        public bool Equals(Bit other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._expression, _expression);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Bit)) return false;
            return Equals((Bit) obj);
        }

        public override int GetHashCode()
        {
            return (_expression != null ? _expression.GetHashCode() : 0);
        }

        public static bool operator ==(Bit left, Bit right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Bit left, Bit right)
        {
            return !left.Equals(right);
        }


        public static implicit operator Bit(bool value)
        {
            return value ? True : False;
        }
        public static bool operator true(Bit value)
        {
            return value == True;
        }
        public static bool operator false(Bit value)
        {
            return value == False;
        }
    }
}