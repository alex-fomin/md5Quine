namespace Bits.Expressions
{
    static class ExpressionExtensions
    {
        public static Operator Dual(this Operator @operator)
        {
            return @operator == Operator.And ? Operator.Or : Operator.And;
        }
    }
}