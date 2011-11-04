using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Bits.Expressions
{
    public class ComplexExpression : Expression, IEnumerable<Expression>
    {
        private readonly Operator _operator;
        private readonly ReadOnlyCollection<Expression> _expressions;

        public ComplexExpression(Operator @operator, IEnumerable<Expression> expressions )
        {
            _operator = @operator;
            _expressions = new List<Expression>(expressions).AsReadOnly();
			Debug.Assert(_expressions.Count > 1);

        }

        public Operator Operator
        {
            get {
                return _operator;
            }
        }

    	public int Count
    	{
			get { return _expressions.Count; }
    	}

    	public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

    	public Expression this[int i]
    	{
    		get { return _expressions[i]; }

    	}

    	public IEnumerator<Expression> GetEnumerator()
    	{
    		return _expressions.GetEnumerator();
    	}

    	public override string ToString()
        {
        	//return "Expression." + Operator + "(" + _expressions.Select(x => x.ToString()).OrderBy(x => x).ToString(", ") +")";
        	return "(" + string.Join(" "+_operator.ToString()+" ", _expressions.Select(x => x.ToString()).OrderBy(x=>x).ToArray()) + ")";
        }

    	IEnumerator IEnumerable.GetEnumerator()
    	{
    		return GetEnumerator();
    	}
    }
}