using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bits.Expressions
{
    public class ComplexExpression : Expression
    {
        private readonly Operator _operator;
        private readonly ReadOnlyCollection<Expression> _expressions;

        public ComplexExpression(Operator @operator, IEnumerable<Expression> expressions )
        {
            _operator = @operator;
            _expressions = new List<Expression>(expressions).AsReadOnly();
        }

        public ReadOnlyCollection<Expression> Expressions
        {
            get { return _expressions; }
        }

        public Operator Operator
        {
            get {
                return _operator;
            }
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }


        public override string ToString()
        {
            return "(" + string.Join(" "+_operator.ToString()+" ", _expressions.Select(x => x.ToString()).OrderBy(x=>x).ToArray()) + ")";
        }
    }
}