namespace Bits.Expressions
{
    public class VariableExpression : Expression
    {
        public string Name { get; private set; }

        public VariableExpression(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(Expression other)
        {
            var variableExpression = other as VariableExpression;
            if (variableExpression == null)
            {
                return false;
            }
            return variableExpression.Name == Name;
        }
    }
}