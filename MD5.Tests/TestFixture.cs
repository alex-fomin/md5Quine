using NUnit.Framework;

namespace MD5.Tests
{
    [TestFixture]
    public class Md5Test
    {
        [TestCase("The quick brown fox jumps over the lazy dog", Result = "9e107d9d372bb6826bd81d3542a419d6")]
        [TestCase("The quick brown fox jumps over the lazy dog.", Result = "e4d909c290d0fb1ca068ffaddf22cbd0")]
        [TestCase("", Result = "d41d8cd98f00b204e9800998ecf8427e")]
        public string CheckMd5(string test)
        {
            var md5 = new Md5 { Value = test };
            return md5.FingerPrint.ToLower();
        }
    }
}
