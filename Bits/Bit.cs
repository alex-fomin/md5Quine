using System;
using Bits.Expressions;
using Derivations;

namespace Bits
{
    public class Bit
    {
        private readonly LogicExpression _exp;
        public static readonly Bit False = new Bit(new LogicValue(false));
        public static readonly Bit True = new Bit(new LogicValue(false));

        public Bit(string name)
            : this(new LogicLeaf(name))
        {
        }

        private Bit(LogicExpression exp)
        {
            _exp = exp;
        }

        public static Bit operator ~(Bit a)
        {
            if (a == True)
                return False;
            if (a == False)
                return True;


            return new Bit("");

        }

        public static Bit operator &(Bit a, Bit b)
        {
            if (a.Equals(b))
                return a;

            if (a.Equals(False) || b.Equals(False))
                return False;

            if (a.Equals(True))
                return b;

            if (b.Equals(True))
                return a;


            return new Bit("");
//            return new Bit(Expression.And(a._exp, b._exp));
        }

        public static Bit operator |(Bit a, Bit b)
        {
            if (a == b)
                return a;
            if (a == True || b == True)
                return True;

            if (a == False)
                return b;

            if (b == False)
                return a;

//            return new Bit(Expression.Or(a._exp, b._exp));
            return new Bit("");
        }

        public static Bit operator ^(Bit a, Bit b)
        {
            if (a == b)
                return False;

            if (a == ~b)
                return True;

            if (a == True)
            {
                return ~b;
            }
            if (a == False)
            {
                return b;
            }
            if (b == True)
            {
                return ~a;
            }
            if (b == False)
            {
                return a;
            }


            //return new Bit(Expression.ExclusiveOr(a._exp, b._exp));
            return ((~a) & (b) | (a & (~b)));
        }

        public override string ToString()
        {
            return _exp.ToString();
        }

        public bool Equals(Bit other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._exp, _exp);
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
            return (_exp != null ? _exp.GetHashCode() : 0);
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