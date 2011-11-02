using System.Linq;
using System.Text;
using Bits.Expressions;

namespace Bits
{
    public class BitVector
    {
        private readonly int _count;
        private readonly Expression[] _bits;

        public BitVector(string s, int count = 32)
        {
            _count = count;
            _bits = Enumerable.Range(0, _count).Select(x => Expression.Variable(s + "_" + x)).ToArray();
        }

        public BitVector(Expression[] bits)
        {
            _bits =  (Expression[]) bits.Clone();
            _count = _bits.Length;            
        }

        public static BitVector operator &(BitVector a, BitVector b)
        {
            var result = new BitVector(a._bits.Zip(b._bits, (a1, b1) => a1 & b1).ToArray());
            return result;
        }
        public static BitVector operator |(BitVector a, BitVector b)
        {
            var result = new BitVector(a._bits.Zip(b._bits, (a1, b1) => a1 | b1).ToArray());
            return result;
        }
        public static BitVector operator ^(BitVector a, BitVector b)
        {
            var result = new BitVector(a._bits.Zip(b._bits, (a1, b1) => a1 ^ b1).ToArray());
            return result;
        }

        public static BitVector operator ~(BitVector a)
        {
            var result = new BitVector(a._bits.Select(x=>~x).ToArray());
            return result;
        }

        public static BitVector operator +(BitVector a, BitVector b)
        {

			var result = new Expression[a._count];

			Expression carry = Expression.False;

            for (int i = 0; i < a._count; i++)
            {
                result[i] = a[i] ^ b[i] ^ carry;
                carry = (a[i] & b[i]) | (a[i] & carry) | (b[i] & carry);
            }

            return new BitVector(result);
        }


        public Expression this[int i]
        {
            get { return _bits[i]; }
        }

        public static explicit operator BitVector(uint b)
        {
            return Convert(b, 32);
        }

        public static explicit operator BitVector(byte b)
        {
            return Convert(b, 8);
        }


        public static implicit operator uint?(BitVector b)
        {
            uint result = 0;
            foreach (var bit in b._bits.Reverse())
            {
                result *= 2;
				if (bit == Expression.True)
                {
                    result += 1;
                }
				else if (bit == Expression.False)
                {
                    result += 0;
                }
                else
                {
                    return null;
                }
            }
            return result;
        }

        public static BitVector UInt32(string name)
        {
            return new BitVector(name, 32);
        }

        private static BitVector Convert(uint u, int count)
        {
        	var r = new Expression[count];
            for (int i = 0; i < count; i++)
            {
				r[i] = Expression.False;
            }
            int j = 0;
            while (u != 0)
            {
                if (u % 2 == 1)
					r[j] = Expression.True;

                u = u/2;
                j++;
            }
            return new BitVector(r);
        }

        public override string ToString()
        {
            uint? x = this;
// ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (x.HasValue)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
                return x.ToString();
            
// ReSharper disable HeuristicUnreachableCode
            var sb = new StringBuilder();
            foreach (var bitV in _bits)
            {
                sb.AppendLine(bitV.ToString());
            }
            return sb.ToString();
// ReSharper restore HeuristicUnreachableCode
        }

        public BitVector RotateLeft(int shift)
        {
            shift = shift%_count;

			var result = new Expression[_count];

            for (int i = 0; i < _count; i++)
            {
                result[(i + shift) % _count] = _bits[i];
            }
            return new BitVector(result);
        }

        public static BitVector operator<<(BitVector value, int shift)
        {
			var result = new Expression[value._count];

            for (int i = 0; i < value._count; i++)
            {
				result[i] = Expression.False;
            }
            for (int i = 0; i < value._count; i++)
            {
                if (i + shift < value._count)
                    result[(i + shift) ] = value._bits[i];
            }
            return new BitVector(result);
        }

        public static BitVector operator >>(BitVector value, int shift)
        {
			var result = new Expression[value._count];

            for (int i = 0; i < value._count; i++)
            {
				result[i] = Expression.False;
            }
            for (int i = 0; i < value._count; i++)
            {
                if (i - shift >=0)
                    result[(i - shift)] = value._bits[i];
            }
            return new BitVector(result);
        }

        public BitVector Revert()
        {
            return new BitVector(_bits.Reverse().ToArray());
        }

        public BitVector Resize(int size)
        {
			var result = new Expression[size];
            for (int i = 0; i < size; i++)
            {
                if (i < _count)
                    result[i] = _bits[i];
                else
                {
					result[i] = Expression.False;
                }
            }
            return new BitVector(result);
        }
    }
}