using System;
using Bits;
using Bits.Expressions;

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

                var bitVector = new BitVector(new Expression[8]
                                                  {
                                                      Expression.Variable("a0"), 
                                                      Expression.Variable("a1"), 
                                                      Expression.False, 
                                                      Expression.False, 
                                                      Expression.False, 
                                                      Expression.False, 
                                                      Expression.False,
                                                      Expression.False
                                                  });
	            var md = new Md5 {ByteInput = new[] {bitVector}};

	            var result = md.FingerPrint;
	            Console.WriteLine(result);

	        }
	    }
	}

