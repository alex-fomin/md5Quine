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
	            var md = new Md5 {ByteInput = new[] {new BitVector("a", 1)}};

	            var result = md.FingerPrint;
	            Console.WriteLine(result);

	        }
	    }
	}

