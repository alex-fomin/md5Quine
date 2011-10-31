#region

using System;

#endregion

namespace Derivations.Exceptions
{
    public class InvalidExpressionException : Exception
    {
        public InvalidExpressionException(String paramString) : base(paramString)
        {
        }
    }
}

/* Location:           D:\Downloads\boolLogicApplet.jar
 * Qualified Name:     com.izyt.boolLogic.InvalidExpressionException
 * JD-Core Version:    0.6.0
 */