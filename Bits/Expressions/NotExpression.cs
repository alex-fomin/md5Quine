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

        public override Expression Simplify()
        {
            var notExpression = _operand as NotExpression;
            if (notExpression != null)
            {
                return notExpression.Operand;
            }

            var valueExpression = _operand as ValueExpression;
            if (valueExpression!=null)
            {
                return valueExpression == ValueExpression.True
                           ? ValueExpression.False
                           : ValueExpression.True;
            }

            return base.Simplify();
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