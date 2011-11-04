using System.Linq;

namespace Bits.Expressions.Laws
{
	public class AbsoptionLaw : ExpressionVisitor
	{
		public override Expression Visit(ComplexExpression complex)
		{
			//for (int i = 0; i < complex.Count; i++)
			//{
			//    Expression left = complex[i];
			//    for (int j = i+1;j<complex.Count;j++)
			//    {
			//        Expression right = complex[j];

			//        var complexRight = right as ComplexExpression;
			//        if (complexRight != null && complex.Operator != complexRight.Operator)
			//        {
			//            if (complexRight.Any(x=>x==left))
			//            {
							
			//            }
			//        }

			//    }
			//}
			return complex;
		}
	}
}