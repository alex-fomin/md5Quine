using System;
using Bits.Expressions;
using Bits.Expressions.Laws;
using NUnit.Framework;

namespace Bits.Test
{
    [TestFixture]
    public class LawTests
    {
        private readonly VariableExpression _a = Expression.Variable("a");
        private readonly VariableExpression _b = Expression.Variable("b");
        private readonly VariableExpression _c = Expression.Variable("c");
        private readonly VariableExpression _d = Expression.Variable("d");
        private readonly VariableExpression _e = Expression.Variable("e");
        private readonly VariableExpression _f = Expression.Variable("f");

        private void Test<T>(Expression start, Expression expected) where T : IExpressionVisitor<Expression>, new()
        {
            Assert.AreNotEqual(expected, start);
            var law = new T();
            var after = start.Accept(law);
            Assert.AreEqual(expected, after);
            var after2 = after.Accept(law);
            Assert.AreSame(after, after2);
        }

        [Test]
        public void AssociativityTest()
        {
            // (a * b) * c = a * b * c 
            Test<AssociativityLaw>((_a | _b) | _c, Expression.Or(_a, _b, _c));
            Test<AssociativityLaw>((_a & _b) & _c, Expression.And(_a, _b, _c));
        }

        [Test]
        public void ComplementationTest()
        {
            Test<ComplementationLaw>(Expression.Or(_a, _b, ~_a), Expression.True);
            Test<ComplementationLaw>(Expression.And(_a, _b, ~_a), Expression.False);
        }

        [Test]
        public void ConstantNegateTest()
        {
            Test<ConstantNegateLaw>(~Expression.True, Expression.False);
            Test<ConstantNegateLaw>(~Expression.False, Expression.True);
        }

        [Test]
        public void DeMorganTest()
        {
            Test<DeMorganLaw>(~(Expression.Or(_a, _b, _c)), Expression.And(~_a, ~_b, ~_c));
            Test<DeMorganLaw>(~(Expression.And(_a, _b, _c)), Expression.Or(~_a, ~_b, ~_c));
        }

        [Test]
        public void DoubleNegationTest()
        {
            Test<DoubleNegationLaw>(~(~(_a | _b)), _a | _b);
        }

        [Test]
        public void DistributionAndTest()
        {
            Test<DistributivityAndLaw>(_a & (_b | _c), (_a & _b) | (_a & _c));
            Test<DistributivityAndLaw>(
                Expression.And(_a, _b | _c, _d), Expression.Or(Expression.And(_a, _b, _d), Expression.And(_a, _c, _d)));


            Test<DistributivityAndLaw>(Expression.And(_a | _b, _c | _d, _e, _f),
                                       Expression.Or(
                                           Expression.And(_a, _c, _e, _f),
                                           Expression.And(_a, _d, _e, _f),
                                           Expression.And(_b, _c, _e, _f),
                                           Expression.And(_b, _d, _e, _f)
                                           ));
            
            Test<DistributivityAndLaw>(Expression.And(_a | _b, _c | _d, _e| _f),
                            Expression.Or(
                                Expression.And(_a, _c, _e),
                                Expression.And(_a, _c, _f),
                                Expression.And(_a, _d, _e),
                                Expression.And(_a, _d, _f),
                                Expression.And(_b, _c, _e),
                                Expression.And(_b, _c, _f),
                                Expression.And(_b, _d, _e),
                                Expression.And(_b, _d, _f)
                                ));

        }

        [Test]
        public void IdempotencyTest()
        {
            Test<IdempotencyLaw>(Expression.Or(_a, _a, _b), _a | _b);
            Test<IdempotencyLaw>(Expression.Or(_a, _a, _a), _a);

            Test<IdempotencyLaw>(Expression.Or(_a, _a, _a, _c), _a | _c);
            Test<IdempotencyLaw>(Expression.Or(_a & _b, _a, _a & _b, _c,_a), Expression.Or(_a & _b, _a, _c));
        }
    }
}