




using System;
using Bits;

namespace MD5
{


	
	/*******************************************************
	 * Programmed by
	 *			 syed Faraz mahmood
	 *					Student NU FAST ICS
	 * can be reached at s_faraz_mahmood@hotmail.com
	 * 
	 * 
	 * 
	 * *******************************************************/

	/// <summary>
	/// constants for md5
	/// </summary>
	public class MD5InitializerConstant
	{
	    public const uint A = 0x67452301;
	    public const uint B = 0xEFCDAB89;
	    public const uint C = 0x98BADCFE;
	    public const uint D = 0X10325476;
	}

	/// <summary>
	/// Represent digest with ABCD
	/// </summary>
	sealed public class Digest
	{
        public BitVector A;
        public BitVector B;
        public BitVector C;
        public BitVector D;

		public Digest()
		{
			A=(BitVector) MD5InitializerConstant.A;
			B=(BitVector) MD5InitializerConstant.B;
			C=(BitVector) MD5InitializerConstant.C;
			D=(BitVector) MD5InitializerConstant.D;
       	}
		public override string ToString()
		{
            uint? a = A.ReverseByte();
            uint? b = B.ReverseByte();
            uint? c = C.ReverseByte();
            uint? d = D.ReverseByte();
            if (a.HasValue && b.HasValue && c.HasValue && d.HasValue)
            {
                string st = a.Value.ToString("X8") +
                            b.Value.ToString("X8") +
                            c.Value.ToString("X8") +
                            d.Value.ToString("X8");
                return st;
            }
            else
            {
                return A.ToString()+B + C + D;
            }
		}
	}


	/// <summary>
	/// helper class providing suporting function
	/// </summary>
	public static class MD5Helper
	{
	    /// <summary>
		/// Left rotates the input word
		/// </summary>
		/// <param name="uiNumber">a value to be rotated</param>
		/// <param name="shift">no of bits to be rotated</param>
		/// <returns>the rotated value</returns>
		public static uint RotateLeft(this uint uiNumber , int shift)
		{
			return ((uiNumber >> 32-shift)|(uiNumber<<shift));
		}


	    /// <summary>
		/// perform a ByteReversal on a number
		/// </summary>
		/// <param name="uiNumber">value to be reversed</param>
		/// <returns>reversed value</returns>
        public static uint ReverseByte(this UInt32 uiNumber)
		{
			return ( (	(uiNumber & 0x000000ff) <<24) |
						(uiNumber >>24) |
					(	(uiNumber & 0x00ff0000) >>8)  |
					(	(uiNumber & 0x0000ff00) <<8) );
		}

	    public static BitVector ReverseByte(this BitVector uiNumber)
	    {
            BitVector u = (uiNumber & (BitVector)0x000000ffu);
	        return ((u << 24) |
                        (uiNumber >> 24) |
                    ((uiNumber & (BitVector)0x00ff0000) >> 8) |
                    ((uiNumber & (BitVector)0x0000ff00) << 8));
	    }
	}
}