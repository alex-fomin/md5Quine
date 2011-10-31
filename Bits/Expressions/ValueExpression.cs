namespace Bits.Expressions
{
    public class ValueExpression : Expression
    {
        public static readonly ValueExpression True = new ValueExpression(true);
        public static readonly ValueExpression False = new ValueExpression(false);

        private readonly bool _value;

        private ValueExpression(bool value)
        {
            _value = value;
        }

        public bool Value
        {
            get { return _value; }
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return _value ? "T" : "F";
        }
    }
}