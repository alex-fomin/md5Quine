#region

using System;

#endregion

namespace Derivations.Exceptions
{
    public class InvalidVariableNameException : Exception
    {
        public InvalidVariableNameException(String paramString) : base(paramString)
        {
        }
    }
}

/* Location:           D:\Downloads\boolLogicApplet.jar
 * Qualified Name:     com.izyt.boolLogic.InvalidVariableNameException
 * JD-Core Version:    0.6.0
 */