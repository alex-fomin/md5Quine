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
                                                         new DistributivityAndLaw(), 
                                                         new AbsorbtionLaw(),
                                                         new AssociativityLaw(),
                                                         new ComplementationLaw(), 
                                                         new ConstantNegateLaw(), 
                                                         new DeMorganLaw(), 
                                                         new DoubleNegationLaw(), 
                                                         new IdentityAnnihilationLaw(), 
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
            bool modified = false;
            var expressions = new List<Expression>();
            foreach (var expression in complex.Expressions)
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



        static Expression Simplify(Expression e)
        {
            Expression before;
            Expression after = e;
            do
            {
                before = after;
                after = _laws.Aggregate(after, (current, law) => current.Accept(law));
            } while (before != after);

            return after;
        }
    }
}