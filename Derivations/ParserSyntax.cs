using Derivations.Exceptions;

namespace Derivations
{
    internal class ParserSyntax
    {
        public ParserSyntax()
            : this(0, NegationSyntax.Unknown)
        {
        }

        public ParserSyntax(LogicSyntax paramInt1, NegationSyntax paramInt2)
        {
            LogicSyntax = paramInt1;
            NegationSyntax = paramInt2;
        }

        public LogicSyntax LogicSyntax { get; private set; }

        public NegationSyntax NegationSyntax { get; private set; }

        public void UpdateLogicSyntax(LogicSyntax paramInt)
        {
            if (((LogicSyntax != 0) && (LogicSyntax != paramInt)) ||
                ((NegationSyntax != NegationSyntax.Unknown) &&
                (LogicParser.GetNegationSyntax(paramInt) != NegationSyntax)))
            {
                throw new DifferentSyntaxesUsedException("Change in logic syntax");
            }

            LogicSyntax = paramInt;
            NegationSyntax = LogicParser.GetNegationSyntax(paramInt);
        }

        public void UpdateNegationSyntax(NegationSyntax paramInt)
        {
            if (((NegationSyntax != NegationSyntax.Unknown) && (NegationSyntax != paramInt)) ||
                ((LogicSyntax != 0) && (LogicParser.GetNegationSyntax(LogicSyntax) != paramInt)))
            {
                throw new DifferentSyntaxesUsedException("Change in logic syntax");
            }

            NegationSyntax = paramInt;
        }
    }
}