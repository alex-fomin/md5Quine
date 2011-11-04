using System;
using System.Collections.Generic;
using System.Linq;
using Bits.Expressions.Laws;

namespace Bits.Expressions
{
    public class Simplifier : ExpressionVisitor
    {
        public static readonly Simplifier Instance = new Simplifier();
        private static readonly ExpressionVisitor[] _laws = new ExpressionVisitor[]
                                                     {
                                                         new DoubleNegationLaw(), 
                                                         new IdentityAnnihilationLaw(), 
                                                         new IdempotencyLaw(),
                                                         new AssociativityLaw(),
                                                         new ComplementationLaw(), 
                                                         new ConstantNegateLaw(), 
                                                         new DeMorganLaw(), 
                                                         new DistributivityAndLaw(), 
                                                     };
        
        
        public override Expression Visit(NotExpression notExpression)
        {
            var operand = notExpression.Operand.Accept(this);
            if (operand != notExpression.Operand)
                notExpression = new NotExpression(operand);

            return Simplify(notExpression);
        }

        public override Expression Visit(ComplexExpression complex)
        {
        	return Process((Expression)complex, SimplifyComplex);
        }

    	private Expression SimplifyComplex(Expression ex)
    	{
    		var complex = ex as ComplexExpression;
			if (complex != null)
			{
				bool modified = false;
				var expressions = new List<Expression>();
				foreach (var expression in complex)
				{
					var visited = expression.Accept(this);
					expressions.Add(visited);
					if (visited != expression)
						modified = true;
				}
				if (modified)
					complex = new ComplexExpression(complex.Operator, expressions);

				Expression resulr = Simplify(complex);
				return resulr;
			}
    		return ex;
    	}


    	static Expression Simplify(Expression e)
        {
        	return Process(e, Update);
        }

    	private static T Process<T>(T e, Func<T,T> processor)
    	{
    		T before;
    		T after = e;
    		do
    		{
    			before = after;
    			after = processor(before);
    		} while ( !Equals(before, after));

    		return after;
    	}

    	private static Expression Update(Expression after)
    	{
    		return _laws.Aggregate(after, (current, law) => current.Accept(law));
    	}
    }
}