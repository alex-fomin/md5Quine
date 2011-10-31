#region

using System;

#endregion

namespace Derivations.Exceptions
{
    public class DifferentSyntaxesUsedException : Exception
    {
        public DifferentSyntaxesUsedException(String paramString)
            : base(paramString)
        {
        }
    }
}