namespace Bits.Expressions
{
    public class VariableExpression : Expression
    {
        private readonly string _name;

        public VariableExpression(string name)
        {
            _name = name;
        }


        public string Name
        {
            get { return _name; }
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return _name;
        }
    }
}