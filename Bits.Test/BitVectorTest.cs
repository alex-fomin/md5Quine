using NUnit.Framework;

namespace Bits.Test
{
    [TestFixture]
    public class BitVectorTest
    {
        [TestCase((uint)1)]
        [TestCase((uint)0)]
        [TestCase((uint)2)]
        [TestCase((uint)200)]
        [TestCase((uint)47)]
        [TestCase(uint.MaxValue)]
        public void Convert(uint value)
        {
            BitVector v = (BitVector)value;
            uint? result = v;
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(value, result.Value);
        }

        [Test]
        public void ConvertUndefined()
        {
            BitVector v = BitVector.UInt32("a");
            uint? result = v;
            Assert.IsFalse(result.HasValue);
        }

        [TestCase(0u, 0u)]
        [TestCase(0u, 20u)]
        [TestCase(145u, 789u)]
        public void SumTest(uint a, uint b)
        {
            BitVector aa = (BitVector)a;
            BitVector bb = (BitVector)b;
            BitVector sum = aa + bb;
            Assert.AreEqual((uint?)sum, a + b);
        }

    }

}