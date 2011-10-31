#region

using System;

#endregion

namespace Derivations
{
    public class ParsedExpression
    {
        public ParsedExpression(String paramString, LogicExpression paramLogicExpression, LogicSyntax paramInt1, NegationSyntax paramInt2)
        {
            InitialText = paramString;
            Expression = paramLogicExpression;
            LogicSyntax = paramInt1;
            NegationSyntax = paramInt2;
        }

        public string InitialText { get; private set; }
        public LogicExpression Expression { get; private set; }
        public LogicSyntax LogicSyntax { get; private set; }
        public NegationSyntax NegationSyntax { get; private set; }
    }
}