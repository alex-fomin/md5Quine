namespace Bits.Expressions
{
    public class NotExpression : Expression
    {
        public Expression Operand { get; private set; }

        public NotExpression(Expression operand)
        {
            Operand = operand;
        }
        public override string ToString()
        {
            return string.Format("Not({0})", Operand);
        }

        public override bool Equals(Expression other)
        {
            var variableExpression = other as NotExpression;
            if (variableExpression == null)
            {
                return false;
            }
            return variableExpression.Operand.Equals(Operand);

        }
    }
}