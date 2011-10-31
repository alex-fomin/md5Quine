namespace Bits.Expressions
{
    class ConstantExpression : Expression
    {
        public bool Value { get; private set; }

        public ConstantExpression(bool value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(Expression other)
        {
            var variableExpression = other as ConstantExpression;
            if (variableExpression == null)
            {
                return false;
            }
            return variableExpression.Value == Value;

        }
    }
}