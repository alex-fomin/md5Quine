using Derivations.Exceptions;

namespace Derivations
{
    public class ParserBlock
    {
        public ParserBlock(LogicExpression paramLogicExpression, LogicSyntax paramInt1, NegationSyntax paramInt2)
        {
            Expression = paramLogicExpression;
            LogicSyntax = paramInt1;
            NegationSyntax = paramInt2;
        }

        public LogicExpression Expression { get; set; }

        public LogicSyntax LogicSyntax { get; set; }

        public NegationSyntax NegationSyntax { get; set; }

        public void Check(LogicSyntax paramInt1, NegationSyntax paramInt2)
        {
            int i = 0;

            int j = paramInt1 != LogicSyntax.f0 ? 1 : 0;

            int k = LogicSyntax != LogicSyntax.f0 ? 1 : 0;

            int m = paramInt2 != 0 ? 1 : 0;

            int n = NegationSyntax != 0 ? 1 : 0;

            if ((j != 0) && (LogicSyntax != paramInt1))
                i = 1;
            else if ((m != 0) && (NegationSyntax != paramInt2))
                i = 1;
            else if ((j != 0) && (n != 0) && (LogicParser.GetNegationSyntax(paramInt1) != NegationSyntax))
            {
                i = 1;
            }
            else if ((k != 0) && (m != 0) && (LogicParser.GetNegationSyntax(LogicSyntax) != paramInt2))
            {
                i = 1;
            }
            if (i != 0)
                throw new DifferentSyntaxesUsedException("Change of logic syntax");
        }
    }
}