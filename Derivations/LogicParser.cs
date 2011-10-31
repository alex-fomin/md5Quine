#region

using System;
using Derivations.Exceptions;

#endregion

namespace Derivations
{
    class LogicSyntaxer
    {
        public static String GetOperatorString(LogicSyntax logic, Operator @operator)
        {
            switch (@operator)
            {
                case Operator.And:
                    return LogicParser.GetAndString(logic);
                case Operator.Or:
                    return LogicParser.GetOrString(logic);
                case Operator.Implies:
                    return LogicParser.GetImpliesString(logic);
                case Operator.Biimplies:
                    return LogicParser.GetBiimpliesString(logic);
                case Operator.Xor:
                    return LogicParser.GetXorString(logic);
            }
            Console.Error.WriteLine("Software Error: Unknown operator passed");
            return LogicParser.GetAndString(logic);
        }
    }

    class NegationSyntaxer
    {
        public static string GetNegation(NegationSyntax paramInt2)
        {
            switch (paramInt2)
            {
                case NegationSyntax.Written:
                    return "NOT ";
                case NegationSyntax.Before:
                    return "~";
                default:
                    throw new ArgumentOutOfRangeException("paramInt2", paramInt2, "Invalid negation");
            }
        }

        public static string GetNegatedString(NegationSyntax negationSyntax, string name)
        {
            switch (negationSyntax)
            {
                case NegationSyntax.Written:
                    return "NOT " + name;
                case NegationSyntax.Before:
                    return "~" + name;
                case NegationSyntax.After:
                    return name + "'";
                default:
                    throw new ArgumentOutOfRangeException("negationSyntax","Unknown negationSyntax passed: " + negationSyntax);
            }
        }
    }



    public enum LogicSyntax
    {
        f_1Invalid = -1,
        NegationSyntax = 0,
        f0 = NegationSyntax,
        
        Written = 1,
        f1 = Written,

        Nu = 2,
        f2=Nu,
        UpDown = 3,
        f3 = UpDown,
        Slashes = 4,
        f4=Slashes,
        DotPlus = 5,
        f5=DotPlus

    }

    public enum NegationSyntax
    {
        Unknown = 10,
        Written = 11,
        Before = 12,
        After = 13,
    }




    public class LogicParser
    {
        private const int bool_INVALID = 20;
        private const int bool_FALSE = 21;
        private const int bool_TRUE = 22;

        public static ParsedExpression ParseLogic(String paramString)
        {
            var localParserSyntax = new ParserSyntax();
            LogicExpression localLogicExpression = Parse(paramString, localParserSyntax);

            var i = localParserSyntax.LogicSyntax;
            var j = localParserSyntax.NegationSyntax;

            if (i == LogicSyntax.f0)
            {
                if (j == NegationSyntax.Unknown)
                {
                    i = LogicSyntax.DotPlus;
                    j = GetNegationSyntax(i);
                }
                else
                {
                    switch (j)
                    {
                        case NegationSyntax.Written:
                            i = LogicSyntax.Written;
                            break;
                        case NegationSyntax.Before:
                        case NegationSyntax.After:
                            i = LogicSyntax.DotPlus;
                            break;
                    }
                }
            }

            return new ParsedExpression(paramString, localLogicExpression, i, j);
        }

        private static LogicExpression Parse(String paramString, ParserSyntax paramParserSyntax)
        {
            if (paramString.Length == 0)
            {
                throw new EmptyLogicException("No expression entered");
            }
            paramString = StripDown(paramString);

            if (paramString.Length == 0)
            {
                throw new EmptyLogicException("Only whitespace or brackets entered");
            }

            String[] arrayOfString = BreakDownInToWords(paramString);

            return ParseWords(arrayOfString, paramParserSyntax);
        }

        private static LogicExpression ParseWords(String[] paramArrayOfString, ParserSyntax paramParserSyntax)
        {
            if (paramArrayOfString.Length == 1)
            {
                String[] arrayOfString = BreakDownInToWords(paramArrayOfString[0]);

                if (arrayOfString.Length == 1)
                {
                    return GenerateLiteral(paramArrayOfString[0], paramParserSyntax);
                }
                paramArrayOfString = arrayOfString;
            }
            int j;
            for (j = paramArrayOfString.Length - 1; j >= 0; j--)
            {
                Operator i = GetOperatorValue(paramArrayOfString[j]);

                if (i == Operator.Implies)
                {
                    var k = GetLogicSyntax(paramArrayOfString[j]);

                    if (k != 0)
                    {
                        paramParserSyntax.UpdateLogicSyntax(k);
                    }
                    return SplitWordsByOperator(paramArrayOfString, j, Operator.Implies, paramParserSyntax);
                }

                if (i == Operator.Biimplies)
                {
                    var k = GetLogicSyntax(paramArrayOfString[j]);

                    if (k != 0)
                    {
                        paramParserSyntax.UpdateLogicSyntax(k);
                    }
                    return SplitWordsByOperator(paramArrayOfString, j, Operator.Biimplies, paramParserSyntax);
                }
            }

            for (j = paramArrayOfString.Length - 1; j >= 0; j--)
            {
                Operator i = GetOperatorValue(paramArrayOfString[j]);

                if (i == Operator.Or)
                {
                    paramParserSyntax.UpdateLogicSyntax(GetLogicSyntax(paramArrayOfString[j]));

                    return SplitWordsByOperator(paramArrayOfString, j, Operator.Or, paramParserSyntax);
                }
                if (paramArrayOfString[j].Equals("!="))
                {
                    return SplitWordsByOperator(paramArrayOfString, j, Operator.Xor, paramParserSyntax);
                }
            }

            for (j = paramArrayOfString.Length - 1; j >= 0; j--)
            {
                if (GetOperatorValue(paramArrayOfString[j]) != 0)
                    continue;
                paramParserSyntax.UpdateLogicSyntax(GetLogicSyntax(paramArrayOfString[j]));

                return SplitWordsByOperator(paramArrayOfString, j, 0, paramParserSyntax);
            }

            if (paramParserSyntax.NegationSyntax == NegationSyntax.Unknown)
            {
                if (paramArrayOfString[0].Equals("NOT", StringComparison.OrdinalIgnoreCase))
                    paramParserSyntax.UpdateNegationSyntax(NegationSyntax.Written);
                else if (paramArrayOfString[0].Equals("~"))
                    paramParserSyntax.UpdateNegationSyntax(NegationSyntax.Before);
                else if (paramArrayOfString[(paramArrayOfString.Length - 1)].Equals("'"))
                {
                    paramParserSyntax.UpdateNegationSyntax(NegationSyntax.After);
                }
                else
                {
                    throw new UnexpectedSymbolException("More than one literal found and no operators");
                }
            }

            j = 0;
            var kk = paramArrayOfString.Length - 1;
            LogicExpression localLogicExpression = null;
            int m;
            if (paramParserSyntax.NegationSyntax == NegationSyntax.Written)
            {
                for (m = 0; m < kk; m++)
                {
                    if (!paramArrayOfString[m].Equals("NOT", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new UnexpectedSymbolException("Expected 'NOT' instead of (or operator after) '" +
                                                            paramArrayOfString[m] + "'");
                    }

                    j = j == 0 ? 1 : 0;
                }

                localLogicExpression = Parse(paramArrayOfString[kk], paramParserSyntax);
            }
            else if (paramParserSyntax.NegationSyntax == NegationSyntax.Before)
            {
                for (m = 0; m < kk; m++)
                {
                    if (!paramArrayOfString[m].Equals("~"))
                    {
                        throw new UnexpectedSymbolException("Expected '~' instead of (or operator after) '" +
                                                            paramArrayOfString[m] + "'");
                    }

                    j = j == 0 ? 1 : 0;
                }

                localLogicExpression = Parse(paramArrayOfString[kk], paramParserSyntax);
            }
            else if (paramParserSyntax.NegationSyntax == NegationSyntax.After)
            {
                for (m = kk; m > 0; m--)
                {
                    if (!paramArrayOfString[m].Equals("'"))
                    {
                        throw new UnexpectedSymbolException("Expected \"'\" instead of (or operator before) '" +
                                                            paramArrayOfString[m] + "'");
                    }

                    j = j == 0 ? 1 : 0;
                }

                localLogicExpression = Parse(paramArrayOfString[0], paramParserSyntax);
            }

            localLogicExpression.Negated = j == 1;
            return localLogicExpression;
        }

        private static LogicExpression SplitWordsByOperator(String[] paramArrayOfString, int position, Operator paramInt2,
                                                            ParserSyntax paramParserSyntax)
        {
            int m;
            if (position == 0)
                throw new UnexpectedSymbolException("Operator at beginning of list of arguments");
            if (position == paramArrayOfString.Length - 1)
            {
                throw new UnexpectedSymbolException("Operator at end of list of arguments");
            }
            if ((paramInt2 == Operator.Implies) || (paramInt2 == Operator.Biimplies) || (paramInt2 == Operator.Xor))
            {
                var arrayOfLogicExpression1 = new LogicExpression[2];

                var arrayOfString1 = new String[position];
                var arrayOfString2 = new String[paramArrayOfString.Length - position - 1];
                for (m = 0; m < arrayOfString1.Length; m++)
                {
                    arrayOfString1[m] = paramArrayOfString[m];
                }
                m = position + 1;

                for (int n = 0; n < arrayOfString2.Length; n++)
                {
                    arrayOfString2[n] = paramArrayOfString[(m++)];
                }
                arrayOfLogicExpression1[0] = ParseWords(arrayOfString1, paramParserSyntax);
                arrayOfLogicExpression1[1] = ParseWords(arrayOfString2, paramParserSyntax);

                var localLogicBranch1 = new LogicBranch(paramInt2, false) {Branches = arrayOfLogicExpression1};
                return localLogicBranch1;
            }

            int i = 2;
            for (int k = position - 1; k >= 0; k--)
            {
                Operator j = GetOperatorValue(paramArrayOfString[k]);

                if (j == Operator.None)
                    continue;
                if (j != paramInt2)
                {
                    break;
                }
                paramParserSyntax.UpdateLogicSyntax(GetLogicSyntax(paramArrayOfString[k]));

                i++;
            }

            var arrayOfLogicExpression2 = new LogicExpression[i];

            m = paramArrayOfString.Length - 1;
            for (int i2 = arrayOfLogicExpression2.Length - 1; i2 >= 0; i2--)
            {
                int i1 = 0;

                int i3;
                for (i3 = m; i3 >= 0; i3--)
                {
                    var j = GetOperatorValue(paramArrayOfString[i3]);

                    if (j != paramInt2)
                        i1++;
                    else if (j == paramInt2)
                    {
                        break;
                    }
                }
                var arrayOfString3 = new String[i1];

                i3 = m - i1;
                int i4 = i1 - 1;

                for (int i5 = m; i5 > i3; i5--)
                {
                    arrayOfString3[(i4--)] = paramArrayOfString[i5];
                }

                m = m - i1 - 1;

                arrayOfLogicExpression2[i2] = ParseWords(arrayOfString3, paramParserSyntax);
            }

            var localLogicBranch2 = new LogicBranch(paramInt2, false) {Branches = arrayOfLogicExpression2};
            return localLogicBranch2;
        }

        private static LogicExpression GenerateLiteral(String paramString, ParserSyntax paramParserSyntax)
        {
            var i = paramParserSyntax.NegationSyntax;

            if (i == NegationSyntax.Unknown)
            {
                if (paramString[0] == '~')
                    paramParserSyntax.UpdateNegationSyntax(NegationSyntax.Before);
                else if (paramString[paramString.Length - 1] == '\'')
                {
                    paramParserSyntax.UpdateNegationSyntax(NegationSyntax.After);
                }
            }
            bool @bool = false;
            int j;
            if (i == NegationSyntax.Before)
            {
                if (paramString[paramString.Length - 1] == '\'')
                {
                    throw new DifferentSyntaxesUsedException("Incorrect negation");
                }

                for (j = 0; (j < paramString.Length) && (paramString[j] == '~'); j++)
                {
                    @bool = !@bool;
                }
                if ((j == paramString.Length - 1) && (paramString[j] == '~'))
                {
                    throw new UnexpectedSymbolException("Negator(s) without literal");
                }
            }
            else if (i == NegationSyntax.After)
            {
                if (paramString[0] == '~')
                {
                    throw new DifferentSyntaxesUsedException("Incorrect negation");
                }

                for (j = paramString.Length - 1; (j >= 0) && (paramString[j] == '\''); j--)
                {
                    @bool = !@bool;
                }
                if ((j == 0) && (paramString[0] == '\''))
                {
                    throw new UnexpectedSymbolException("Negator(s) without literal");
                }
            }

            LogicExpression localLogicExpression = GenerateLiteral(paramString);
            localLogicExpression.Negated = @bool;
            return localLogicExpression;
        }

        private static LogicExpression GenerateLiteral(String paramString)
        {
            switch (GetValidboolValue(paramString))
            {
                case 22:
                    return new LogicValue(true);
                case 21:
                    return new LogicValue(false);
            }

            if (IsValidVariableName(paramString))
            {
                return new LogicLeaf(paramString);
            }
            throw new InvalidVariableNameException("Variable name not allowed: '" + paramString + "'");
        }

        private static int GetValidboolValue(String paramString)
        {
            if (paramString.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                return 22;
            if (paramString.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
            {
                return 21;
            }
            return 20;
        }

        private static bool IsValidVariableName(String paramString)
        {
            if (paramString.Length == 0)
            {
                return false;
            }
            char c = paramString[0];

            if (GetOperatorValue(paramString) != Operator.None)
            {
                return false;
            }
            if (!IsAlphaChar(c))
            {
                return false;
            }
            int i = paramString.Length;

            for (int j = 1; j < i; j++)
            {
                c = paramString[j];

                if ((!IsAlphaChar(c)) && (!IsNumChar(c)) && (c != '-') && (c != '_'))
                {
                    return false;
                }
            }
            return true;
        }

        private static LogicSyntax GetLogicSyntax(String paramString)
        {
            if (paramString.Equals("AND", StringComparison.OrdinalIgnoreCase) ||
                paramString.Equals("OR", StringComparison.OrdinalIgnoreCase) ||
                paramString.Equals("XOR", StringComparison.OrdinalIgnoreCase) ||
                paramString.Equals("IMPLIES", StringComparison.OrdinalIgnoreCase) ||
                paramString.Equals("BIIMPLIES", StringComparison.OrdinalIgnoreCase) ||
                paramString.Equals("NOT", StringComparison.OrdinalIgnoreCase))
            {
                return LogicSyntax.Written;
            }
            if (paramString.Equals("n", StringComparison.OrdinalIgnoreCase) ||
                paramString.Equals("u", StringComparison.OrdinalIgnoreCase))
                return LogicSyntax.f2;
            if (paramString.Equals("^") || paramString.Equals("v", StringComparison.OrdinalIgnoreCase))
                return LogicSyntax.f3;
            if (paramString.Equals("/\\") || paramString.Equals("\\/"))
                return LogicSyntax.f4;
            if (paramString.Equals(".") || paramString.Equals("+"))
                return LogicSyntax.f5;
            if (paramString.Equals("=>") || paramString.Equals("<=>") || paramString.Equals("!="))
            {
                return 0;
            }
            return LogicSyntax.f_1Invalid;
        }

        public static NegationSyntax GetNegationSyntax(LogicSyntax paramInt)
        {
            switch (paramInt)
            {
                case LogicSyntax.f1:
                    return NegationSyntax.Written;
                case LogicSyntax.f2:
                    return NegationSyntax.Before;
                case LogicSyntax.f3:
                    return NegationSyntax.Before;
                case LogicSyntax.f4:
                    return NegationSyntax.Before;
                case LogicSyntax.f5:
                    return NegationSyntax.After;
                case LogicSyntax.f0:
                    return NegationSyntax.Unknown;
            }
            Console.Error.WriteLine("Software error: unknown negationSyntax; logicSyntax: " + paramInt);

            return NegationSyntax.Unknown;
        }



        private static Operator GetOperatorValue(String paramString)
        {
            if (paramString.Equals("AND", StringComparison.OrdinalIgnoreCase) ||
                paramString.Equals("n", StringComparison.OrdinalIgnoreCase) ||
                paramString.Equals("^") || paramString.Equals("/\\") || paramString.Equals("."))
            {
                return Operator.And;
            }
            if (paramString.Equals("OR", StringComparison.OrdinalIgnoreCase) ||
                paramString.Equals("u", StringComparison.OrdinalIgnoreCase) ||
                paramString.Equals("v", StringComparison.OrdinalIgnoreCase) || paramString.Equals("\\/") ||
                paramString.Equals("+"))
            {
                return Operator.Or;
            }
            if (paramString.Equals("XOR", StringComparison.OrdinalIgnoreCase) || paramString.Equals("!="))
                return Operator.Xor;
            if (paramString.Equals("IMPLIES", StringComparison.OrdinalIgnoreCase) || paramString.Equals("=>"))
                return Operator.Implies;
            if (paramString.Equals("BIIMPLIES", StringComparison.OrdinalIgnoreCase) || paramString.Equals("<=>"))
            {
                return Operator.Biimplies;
            }
            return Operator.None;
        }

        private static String StripDown(String paramString)
        {
            paramString = paramString.Trim();

            while ((paramString.Length > 0) && (paramString[0] == '('))
            {
                if (FindMatchingBracket(paramString, 0) != paramString.Length - 1)
                    break;
                paramString = paramString.Substring(1, paramString.Length - 2);
                paramString = paramString.Trim();
            }

            return paramString;
        }

        private static int FindMatchingBracket(String paramString, int paramInt)
        {
            int i = 1;
            int j = paramInt;

            while (i > 0)
            {
                int k = paramString.IndexOf('(', j + 1);
                int m = paramString.IndexOf(')', j + 1);

                if (m == -1)
                    throw new UnclosedBracketException();
                if ((m < k) || (k == -1))
                {
                    j = m;
                    i--;
                    continue;
                }

                j = k;
                i++;
            }

            return j;
        }

        private static String[] BreakDownInToWords(String paramString)
        {
            var arrayOfString = new String[0];

            for (int i = 0; i < 2; i++)
            {
                int j = i == 1 ? 1 : 0;
                int k = 0;

                for (int m = 0; m < paramString.Length; m++)
                {
                    char c = paramString[m];
                    int n;
                    if (c == '(')
                    {
                        n = FindMatchingBracket(paramString, m);

                        if (j != 0)
                            arrayOfString[(k++)] = StripDown(paramString.Substring(m + 1, n-m-1));
                        else
                        {
                            k++;
                        }
                        m = n;
                    }
                    else
                    {
                        if (IsWhiteSpace(c))
                        {
                            n = paramString.Length;

                            int i1;
                            for (i1 = m + 1; i1 < paramString.Length; i1++)
                            {
                                if (IsWhiteSpace(paramString[i1]))
                                    continue;
                                n = i1 - 1;
                                break;
                            }

                            m = n;
                        }
                        else if ((c == '^') || (c == 'v') || (c == '.') || (c == '+') ||
                                 (((c == 'n') || (c == 'u')) && (m + 1 < paramString.Length) &&
                                  (IsWhiteSpace(paramString[m + 1]))))
                        {
                            if (j != 0)
                            {
                                arrayOfString[(k++)] = paramString.Substring(m, 1);
                            }
                            else
                                k++;
                        }
                        else if (c == '/')
                        {
                            if (m + 2 >= paramString.Length)
                            {
                                throw new UnexpectedSymbolException(
                                    "Unexpected forward-slash (/) found at end of expression");
                            }

                            if (paramString[m + 1] == '\\')
                            {
                                if (j != 0)
                                {
                                    arrayOfString[(k++)] = paramString.Substring(m, 2);
                                }
                                else
                                {
                                    k++;
                                }
                                m++;
                            }
                            else
                            {
                                throw new UnexpectedSymbolException("Incomplete AND (/\\) operator: " +
                                                                    paramString.Substring(m,  2));
                            }
                        }
                        else if (c == '\\')
                        {
                            if (m + 2 >= paramString.Length)
                            {
                                throw new UnexpectedSymbolException(
                                    "Unexpected back-slash (\\) found at end of expression");
                            }

                            if (paramString[m + 1] == '/')
                            {
                                if (j != 0)
                                {
                                    arrayOfString[(k++)] = paramString.Substring(m, 2);
                                }
                                else
                                {
                                    k++;
                                }
                                m++;
                            }
                            else
                            {
                                throw new UnexpectedSymbolException("Incomplete OR (\\/) operator: " +
                                                                    paramString.Substring(m,  2));
                            }
                        }
                        else if (c == '=')
                        {
                            if (m + 2 >= paramString.Length)
                            {
                                throw new UnexpectedSymbolException(
                                    "Unexpected equals (=) found at end of expression");
                            }

                            if (paramString[m + 1] == '>')
                            {
                                if (j != 0)
                                {
                                    arrayOfString[(k++)] = paramString.Substring(m,  2);
                                }
                                else
                                {
                                    k++;
                                }
                                m++;
                            }
                            else
                            {
                                throw new UnexpectedSymbolException("Incomplete => (IMPLIES) operator: " +
                                                                    paramString.Substring(m,  2));
                            }
                        }
                        else if (c == '<')
                        {
                            if (m + 3 >= paramString.Length)
                            {
                                throw new UnexpectedSymbolException(
                                    "Unexpected equals (<) found at end of expression");
                            }

                            if ((paramString[m + 1] == '=') && (paramString[m + 2] == '>'))
                            {
                                if (j != 0)
                                {
                                    arrayOfString[(k++)] = paramString.Substring(m,  3);
                                }
                                else
                                {
                                    k++;
                                }
                                m += 2;
                            }
                            else
                            {
                                throw new UnexpectedSymbolException("Incomplete <=> (BI-IMPLICATION) operator: " +
                                                                    paramString.Substring(m,  2));
                            }
                        }
                        else if (c == '!')
                        {
                            if (m + 2 >= paramString.Length)
                            {
                                throw new UnexpectedSymbolException(
                                    "Unexpected exclamation (!) found at end of expression");
                            }

                            if (paramString[m + 1] == '=')
                            {
                                if (j != 0)
                                {
                                    arrayOfString[(k++)] = paramString.Substring(m,  2);
                                }
                                else
                                {
                                    k++;
                                }
                                m++;
                            }
                            else
                            {
                                throw new UnexpectedSymbolException("Incomplete != (XOR) operator: " +
                                                                    paramString.Substring(m,  2));
                            }
                        }
                        else if ((c == '~') || (c == '\''))
                        {
                            if (j != 0)
                                arrayOfString[(k++)] = Char.ToString(c);
                            else
                            {
                                k++;
                            }
                        }
                        else
                        {
                            if ((!IsAlphaChar(c)) && (!IsNumChar(c)))
                            {
                                throw new UnexpectedSymbolException("Invalid char: '" + c + "'");
                            }
                            n = paramString.Length;

                            for (int i2 = m + 1; i2 < paramString.Length; i2++)
                            {
                                var i11 = paramString[i2];

                                if ((IsAlphaChar(i11)) || (IsNumChar(i11)) || (i11 == 45) || (i11 == 95))
                                {
                                    continue;
                                }
                                n = i2;
                                break;
                            }

                            if (j != 0)
                            {
                                arrayOfString[(k++)] = paramString.Substring(m, n-m);
                            }
                            else
                            {
                                k++;
                            }
                            m = n - 1;
                        }
                    }
                }
                if (j == 0)
                {
                    arrayOfString = new String[k];
                }
            }
            return arrayOfString;
        }

        private static bool IsAlphaChar(char paramChar)
        {
            return ((paramChar >= 'a') && (paramChar <= 'z')) || ((paramChar >= 'A') && (paramChar <= 'Z'));
        }

        private static bool IsNumChar(char paramChar)
        {
            return (paramChar >= '0') && (paramChar <= '9');
        }

        private static bool IsWhiteSpace(char paramChar)
        {
            return (paramChar == ' ') || (paramChar == '\t');
        }

        public static String GetAndString(LogicSyntax paramInt)
        {
            switch (paramInt)
            {
                case LogicSyntax.f1:
                    return "AND";
                case LogicSyntax.f2:
                    return "n";
                case LogicSyntax.f3:
                    return "^";
                case LogicSyntax.f4:
                    return "/\\";
                case LogicSyntax.f5:
                    return ".";
            }
            return ".";
        }



        public static String GetOrString(LogicSyntax paramInt)
        {
            switch (paramInt)
            {
                case LogicSyntax.f1:
                    return "OR";
                case LogicSyntax.f2:
                    return "u";
                case LogicSyntax.f3:
                    return "v";
                case LogicSyntax.f4:
                    return "\\/";
                case LogicSyntax.f5:
                    return "+";
            }
            return "+";
        }

        public static String GetXorString(LogicSyntax paramInt)
        {
            switch (paramInt)
            {
                case LogicSyntax.f1:
                    return "XOR";
            }
            return "!=";
        }

        public static String GetImpliesString(LogicSyntax paramInt)
        {
            switch (paramInt)
            {
                case LogicSyntax.f1:
                    return "IMPLIES";
            }
            return "=>";
        }

        public static String GetBiimpliesString(LogicSyntax paramInt)
        {
            switch (paramInt)
            {
                case LogicSyntax.f1:
                    return "BIIMPLIES";
            }
            return "<=>";
        }
    }

    public enum Operator
    {
        And = 0,
        Or = 1,
        Xor = 4,
        Implies = 2,
        Biimplies = 3,
        None = -1
    }
}

/* Location:           D:\Downloads\boolLogicApplet.jar
 * Qualified Name:     com.izyt.boolLogic.LogicParser
 * JD-Core Version:    0.6.0
 */