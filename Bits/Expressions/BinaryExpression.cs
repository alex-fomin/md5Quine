namespace Bits.Expressions
{
    public class BinaryExpression : Expression
    {
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }
        public ExpressionType ExpressionType { get; private set; }

        public BinaryExpression(Expression left, Expression right, ExpressionType expressionType)
        {
            ExpressionType = expressionType;
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return string.Format("({0} {1} {2})", Left, ExpressionType, Right);
        }

        public override bool Equals(Expression other)
        {
            var variableExpression = other as BinaryExpression;
            if (variableExpression == null)
            {
                return false;
            }
            return (variableExpression.ExpressionType == ExpressionType) &&
                
                
              ((  (variableExpression.Left.Equals(Left)) &&
                (variableExpression.Right.Equals(Right)))||(  (variableExpression.Left.Equals(Right)) &&
                (variableExpression.Right.Equals(Left))));

        }
    }
}