namespace Bits.Expressions
{
    public class ValueExpression : Expression
    {

        private readonly bool _value;

    	public ValueExpression(bool value)
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