namespace Bits.Expressions
{
    public class NotExpression : Expression
    {
        private readonly Expression _operand;

        public NotExpression(Expression operand)
        {
            _operand = operand;
        }

        public Expression Operand
        {
            get { return _operand; }
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return string.Format("Not({0})", Operand);
        }
    }
}