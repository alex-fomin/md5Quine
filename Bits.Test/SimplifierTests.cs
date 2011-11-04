using System;
using Bits.Expressions;
using NUnit.Framework;

namespace Bits.Test
{
	[TestFixture]
	public class SimplifierTests
	{
		private static Expression a0 = Expression.Variable("a");
		private static Expression a1 = Expression.Variable("b");
		private static Expression T = Expression.True;
		private static Expression F = Expression.False;

		[Test]
		public void Simplify()
		{
			Expression x = Expression.Or(Expression.And(Expression.Or(a0, a1, Expression.And(a0, a1)), Expression.Not(T)),
			                             Expression.And(Not(Expression.Or(a0, a1, Expression.And(a0, a1))), T));

			var y = x.Simlpify();
			Console.WriteLine(y);
		}

		private Expression Not(Expression expression)
		{
			return Expression.Not(expression);
		}
	}
}