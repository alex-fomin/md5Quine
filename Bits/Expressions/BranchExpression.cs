using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bits.Expressions
{
    public abstract class BranchExpression : Expression
    {
        private readonly Operator _operator;
        private readonly HashSet<Expression> _expressions;

        protected BranchExpression(Operator @operator, IEnumerable<Expression> expressions )
        {
            _operator = @operator;
            _expressions = new HashSet<Expression>(expressions, ExpressionComparer.Default);
        }

        public ISet<Expression> Expressions
        {
            get { return _expressions; }
        }

        public Operator Operator
        {
            get {
                return _operator;
            }
        }

        public override Expression Simplify()
        {
            Expression value;
            if (TrySimplify(out value))
                return value;


            return base.Simplify();
        }

        private bool TrySimplify(out Expression value)
        {
            value = null;
            var inner = new List<Expression>();
            foreach (var expression in Expressions)
            {
                var branchExpression = expression as BranchExpression;
                if (branchExpression == null)
                    return false;

                if (branchExpression.Operator != Operator)
                    return false;

                inner.AddRange(branchExpression.Expressions);
            }
            Expression[] expressions = inner.ToArray();
            switch (Operator)
            {
                case Operator.And:
                    value= And(expressions);
                    break;
                case Operator.Or:
                    value= Or(expressions);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }

        public override string ToString()
        {
            return "(" + string.Join(" "+_operator.ToString()+" ", _expressions.Select(x => x.ToString()).ToArray()) + ")";
        }
    }
}