using NUnit.Framework;

namespace Bits.Test
{
    [TestFixture]
    public class BitTests
    {

        [Test, TestCaseSource("_andSource")]
        public void AndTest(Bit a, Bit b, Bit r)
        {
            Assert.AreEqual(a & b, r);
        }
        [Test, TestCaseSource("_orSource")]
        public void OrTest(Bit a, Bit b, Bit r)
        {
            Assert.AreEqual(a | b, r);
        }


// ReSharper disable UnusedMember.Local
        private static object[] _andSource = new[]
                                                {
                                                    new[] {Bit.True, Bit.True, Bit.True},
                                                    new[] {Bit.True, Bit.False, Bit.False},
                                                    new[] {Bit.False, Bit.True, Bit.False},
                                                    new[] {Bit.False, Bit.False, Bit.False},
                                                };
        private static object[] _orSource = new[]
                                                {
                                                    new[] {Bit.True, Bit.True, Bit.True},
                                                    new[] {Bit.True, Bit.False, Bit.True},
                                                    new[] {Bit.False, Bit.True, Bit.True},
                                                    new[] {Bit.False, Bit.False, Bit.False},
                                                };
        // ReSharper restore UnusedMember.Local
    }

}
