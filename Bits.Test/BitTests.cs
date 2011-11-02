using Bits.Expressions;
using NUnit.Framework;

namespace Bits.Test
{
    [TestFixture]
    public class BitTests
    {

        [Test, TestCaseSource("_andSource")]
		public void AndTest(Expression a, Expression b, Expression r)
        {
            Assert.AreEqual(a & b, r);
        }
        [Test, TestCaseSource("_orSource")]
		public void OrTest(Expression a, Expression b, Expression r)
        {
            Assert.AreEqual(a | b, r);
        }


// ReSharper disable UnusedMember.Local
        private static object[] _andSource = new[]
                                                {
                                                    new[] {Expression.True, Expression.True, Expression.True},
                                                    new[] {Expression.True, Expression.False, Expression.False},
                                                    new[] {Expression.False, Expression.True, Expression.False},
                                                    new[] {Expression.False, Expression.False, Expression.False},
                                                };
        private static object[] _orSource = new[]
                                                {
                                                    new[] {Expression.True, Expression.True, Expression.True},
                                                    new[] {Expression.True, Expression.False, Expression.True},
                                                    new[] {Expression.False, Expression.True, Expression.True},
                                                    new[] {Expression.False, Expression.False, Expression.False},
                                                };
        // ReSharper restore UnusedMember.Local
    }

}
