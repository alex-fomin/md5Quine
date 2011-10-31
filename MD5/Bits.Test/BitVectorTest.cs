using System;
using MD5;
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


        [TestCase(1u, 1)]
        [TestCase(18654u, 5)]
        [TestCase(uint.MaxValue - 100u, 10)]
        public void RotateLeftTest(uint a, int s)
        {
            BitVector aa = (BitVector)a;
            BitVector raa = aa.RotateLeft(s);

            uint ra = a.RotateLeft(s);

            Assert.AreEqual((uint)raa, ra);
        }


        [TestCase(1u)]
        [TestCase(18654u)]
        [TestCase(uint.MaxValue - 100u)]
        public void ReverseTest(uint a)
        {
            BitVector aa = (BitVector)a;
            BitVector raa = aa.ReverseByte();

            uint ra = a.ReverseByte();

            Assert.AreEqual((uint)raa, ra);
        }
    }

}