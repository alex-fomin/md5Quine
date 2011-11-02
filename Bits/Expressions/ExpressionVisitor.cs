namespace Bits.Expressions
{
    public class ExpressionVisitor : IExpressionVisitor<Expression>
    {
        public virtual Expression Visit(NotExpression notExpression)
        {
            return notExpression;
        }

        public virtual Expression Visit(VariableExpression variableExpression)
        {
            return variableExpression;
        }

        public virtual Expression Visit(ValueExpression valueExpression)
        {
            return valueExpression;
        }

        public virtual Expression Visit(ComplexExpression complex)
        {
            return complex;
        }
    }
}