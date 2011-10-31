#region

using System;

#endregion

namespace Derivations.Exceptions
{
    public class UnclosedBracketException : Exception
    {
        public UnclosedBracketException()
        {
        }

        public UnclosedBracketException(String paramString) : base(paramString)
        {
        }
    }
}