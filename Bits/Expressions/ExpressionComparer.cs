using System;
using System.Collections.Generic;
using System.Linq;

namespace Bits.Expressions
{
    class ExpressionComparer : IEqualityComparer<Expression>
    {
        public static readonly ExpressionComparer Default = new ExpressionComparer();

        public bool Equals(Expression x, Expression y)
        {
            if (ReferenceEquals(x, y))
                return true;
            
            if (x.GetType() != y.GetType())
                return false;

            if (x is NotExpression)
                return Equals((NotExpression)x, (NotExpression)y);
            if (x is VariableExpression)
                return Equals((VariableExpression)x, (VariableExpression)y);
            if (x is ValueExpression)
                return Equals((ValueExpression)x, (ValueExpression)y);
            if (x is ComplexExpression)
                return Equals((ComplexExpression)x, (ComplexExpression)y);

            throw new InvalidOperationException("Invalid expression type");
        }


        public bool Equals(NotExpression a, NotExpression b)
        {
            return Equals(a.Operand, b.Operand);
        }
        public bool Equals(VariableExpression a, VariableExpression b)
        {
            return a.Name == b.Name;
        }

        public bool Equals(ValueExpression a, ValueExpression b)
        {
            return a.Value == b.Value;
        }

        public bool Equals(ComplexExpression a, ComplexExpression b)
        {
            if (a.Operator != b.Operator)
                return false;
            if (a.Count != b.Count)
                return false;

            return !a.Except(b, this).Any();
        }

        static readonly HashVisitor _hashVisitor = new HashVisitor();
        public int GetHashCode(Expression obj)
        {
            return obj.Accept(_hashVisitor);
        }

        
        class HashVisitor : IExpressionVisitor<int>
        {
            public int Visit(NotExpression notExpression)
            {
                return ~notExpression.Operand.Accept(this);
            }

            public int Visit(VariableExpression variableExpression)
            {
                return variableExpression.Name.GetHashCode();
            }

            public int Visit(ValueExpression valueExpression)
            {
                return valueExpression.Value.GetHashCode();
            }

            public int Visit(ComplexExpression complex)
            {
                return complex.Aggregate(complex.Operator.GetHashCode()*397, (current, expression) => current ^ expression.Accept(this));
            }
        }
    }
}