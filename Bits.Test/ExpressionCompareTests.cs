using Bits.Expressions;
using NUnit.Framework;

namespace Bits.Test
{
    [TestFixture]
    public class ExpressionCompareTests
    {
        private readonly VariableExpression _a = Expression.Variable("a");
        private readonly VariableExpression _b = Expression.Variable("b");
        private readonly VariableExpression _c = Expression.Variable("c");

        [Test]
        public void CommutativityTest()
        {
            Assert.AreEqual(Expression.Or(_a, _b, _c), Expression.Or(_b, _a, _c));
        }
    }
}