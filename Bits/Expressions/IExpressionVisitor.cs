namespace Bits.Expressions
{
    public interface IExpressionVisitor<out T>
    {
        T Visit(NotExpression notExpression);
        T Visit(VariableExpression variableExpression);
        T Visit(ValueExpression valueExpression);
        T Visit(ComplexExpression complex);
    }
}