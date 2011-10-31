using System;
using Bits;

/*******************************************************
 * Programmed by
 *			 syed Faraz mahmood
 *			 	Student NU FAST ICS
 * can be reached at s_faraz_mahmood@hotmail.com
 * 
 * 
 * 
 * *******************************************************/
	namespace MD5
	{
	    /// <summary>
	    /// test driver for the MD5
	    /// </summary>
	    public class MainClass
	    {
	        public static void Main()
	        {

                var bitVector = new BitVector(new Bit[8]
                                                  {
                                                      new Bit("a0"), 
                                                      new Bit("a1"), 
                                                      Bit.False, 
                                                      Bit.False, 
                                                      Bit.False, 
                                                      Bit.False, 
                                                      Bit.False,
                                                      Bit.False
                                                  });
	            var md = new Md5 {ByteInput = new[] {bitVector}};

	            var result = md.FingerPrint;
	            Console.WriteLine(result);

	        }
	    }
	}

