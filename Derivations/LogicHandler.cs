#region

using System;
using System.Linq;

#endregion

namespace Derivations
{
    public class LogicHandler
    {
        public static int Main(String[] paramArrayOfString)
        {
            Console.WriteLine("# LogicHandler v1.0 beta by Isaac Turner");
            Console.WriteLine("# isaac.turner@student.manchester.ac.uk");

            //if (paramArrayOfString.Length != 1)
            //{
            //    Console.WriteLine("USAGE: java -jar boolLogic.jar <expression>");
            //    return 0;
            //}

            const string paramString = "(b0.b0).(a+(b.(c+d))).d.a"; //paramArrayOfString[0];
            try
            {
                ParsedExpression localParsedExpression = LogicParser.ParseLogic(paramString);

                LogicDerivation localLogicDerivation = ManipulateLogic(localParsedExpression);
                localLogicDerivation.PrintTrace(localParsedExpression.LogicSyntax,
                                                localParsedExpression.NegationSyntax);
            }
            catch (Exception localException)
            {
                Console.Error.WriteLine(paramString);
                Console.Error.WriteLine(localException.Message);

                return -1;
            }
            return 0;
        }

        public static LogicDerivation ManipulateLogic(ParsedExpression paramParsedExpression)
        {
            return ManipulateLogic(paramParsedExpression.InitialText, paramParsedExpression.Expression);
        }

        public static LogicDerivation ManipulateLogic(String paramString, LogicExpression paramLogicExpression)
        {
            var localLogicDerivation = new LogicDerivation(paramString, paramLogicExpression);

            if (localLogicDerivation.CNFAndDNF)
            {
                return localLogicDerivation;
            }

            CarryOutNonPrimaryOperatorReplacement(localLogicDerivation);
            CarryOutBoolValues(localLogicDerivation);

            if (localLogicDerivation.CNFAndDNF)
            {
                return localLogicDerivation;
            }
            CarryOutAssociativity(localLogicDerivation);
            CarryOutDeMorgans(localLogicDerivation);
            CarryOutAssociativity(localLogicDerivation);
            CarryOutIdempotency(localLogicDerivation);
            CarryOutBoolValues(localLogicDerivation);
            CarryOutAbsorbtion(localLogicDerivation);

            do
            {
                CarryOutDistributivity(localLogicDerivation);
                CarryOutAssociativity(localLogicDerivation);
                CarryOutIdempotency(localLogicDerivation);
                CarryOutBoolValues(localLogicDerivation);
                CarryOutAbsorbtion(localLogicDerivation);
            } while (!localLogicDerivation.CNFAndDNF);
            return localLogicDerivation;
        }


        public static String GetFormName(Form paramInt)
        {
            switch (paramInt)
            {
                case Form.None:
                    return "Not any Normal Form";
                case Form.NegationNormalForm:
                    return "Negation Normal Form";
                case Form.CNF:
                    return "Conjunctive Normal Form (CNF)";
                case Form.DNF:
                    return "Disjunctive Normal Form (DNF)";
                case Form.CNF_DNF:
                    return "Conjunctive and Disjunctive Normal Form (CNF & DNF)";
            }

            return "Invalid Form passed";
        }

        private static void CarryOutNonPrimaryOperatorReplacement(LogicDerivation paramLogicDerivation)
        {
            LogicExpression localLogicExpression1 = paramLogicDerivation.Next;

            int i = localLogicExpression1.GetDepth();

            for (int k = 2; k <= i; k++)
            {
                int j = 0;
                LogicExpression localLogicExpression2;
                while ((localLogicExpression2 = localLogicExpression1.GetSubExpression(k, j++)) != null)
                {
                    var localLogicBranch = (LogicBranch) localLogicExpression2;

                    switch (localLogicBranch.Operator)
                    {
                        case Operator.Implies:
                            ReplaceImpliesOperator(localLogicBranch);
                            j += 2;
                            paramLogicDerivation.AddStep(localLogicExpression1, "Replaced IMPLIES operator");

                            localLogicExpression1 = paramLogicDerivation.Next;
                            break;
                        case Operator.Biimplies:
                            ReplaceBiimpliesOperator(localLogicBranch);
                            j++;
                            paramLogicDerivation.AddStep(localLogicExpression1, "Replaced BIIMPLIES operator");

                            localLogicExpression1 = paramLogicDerivation.Next;
                            break;
                        case Operator.Xor:
                            ReplaceXorOperator(localLogicBranch);
                            j++;
                            paramLogicDerivation.AddStep(localLogicExpression1, "Replaced XOR operator");

                            localLogicExpression1 = paramLogicDerivation.Next;
                            break;
                        default:
                            j++;
                            break;
                    }
                }
            }
        }

        private static void CarryOutDistributivity(LogicDerivation paramLogicDerivation)
        {
            LogicExpression localLogicExpression1 = paramLogicDerivation.Next;

            int i = 0;
            LogicExpression localLogicExpression2;
            while ((localLogicExpression2 = localLogicExpression1.GetSubExpression(3, i++)) != null)
            {
                if (!Distributivity((LogicBranch) localLogicExpression2))
                    continue;
                paramLogicDerivation.AddStep(localLogicExpression1, "Distributivity");
                localLogicExpression1 = paramLogicDerivation.Next;
            }
        }

        private static void CarryOutAbsorbtion(LogicDerivation paramLogicDerivation)
        {
            Object localObject = paramLogicDerivation.Next;

            int i = 0;
            LogicExpression localLogicExpression1;
            while ((localLogicExpression1 = ((LogicExpression) localObject).GetSubExpression(3, i)) != null)
            {
                var localLogicBranch1 = (LogicBranch) localLogicExpression1;
                int j = localLogicBranch1.Branches.Length;
                int k = Absorbtion(localLogicBranch1);

                if (k > 0)
                {
                    if (j - k == 1)
                    {
                        LogicBranch localLogicBranch2 = localLogicExpression1.Parent;

                        LogicExpression[] arrayOfLogicExpression = localLogicBranch1.Branches;
                        LogicExpression localLogicExpression2 = arrayOfLogicExpression[0];

                        if (localLogicBranch2 == null)
                        {
                            localObject = localLogicExpression2;
                            ((LogicExpression) localObject).SetParent(null, -1);
                        }
                        else
                        {
                            localLogicBranch2.SetBranch(localLogicExpression2, localLogicBranch1.PositionInParent);
                        }

                        i--;
                    }

                    paramLogicDerivation.AddStep((LogicExpression) localObject, "Absorbtion");
                    localObject = paramLogicDerivation.Next;
                    continue;
                }

                i++;
            }
        }

        private static void CarryOutDeMorgans(LogicDerivation paramLogicDerivation)
        {
            LogicExpression localLogicExpression1 = paramLogicDerivation.Next;

            int i = localLogicExpression1.GetDepth();

            for (int k = i; k >= 2; k--)
            {
                int j = 0;
                LogicExpression localLogicExpression2;
                while ((localLogicExpression2 = localLogicExpression1.GetSubExpression(k, j)) != null)
                {
                    if (DeMorgans((LogicBranch) localLogicExpression2))
                    {
                        paramLogicDerivation.AddStep(localLogicExpression1, "De Morgan's");
                        localLogicExpression1 = paramLogicDerivation.Next;
                        continue;
                    }

                    j++;
                }
            }
        }

        private static void CarryOutAssociativity(LogicDerivation paramLogicDerivation)
        {
            LogicExpression localLogicExpression1 = paramLogicDerivation.Next;

            int i = localLogicExpression1.GetDepth();

            for (int k = 3; k <= i; k++)
            {
                int j = 0;
                LogicExpression localLogicExpression2;
                while ((localLogicExpression2 = localLogicExpression1.GetSubExpression(k, j)) != null)
                {
                    if (associativity((LogicBranch) localLogicExpression2))
                    {
                        paramLogicDerivation.AddStep(localLogicExpression1, "Associativity");
                        localLogicExpression1 = paramLogicDerivation.Next;
                        continue;
                    }

                    j++;
                }
            }

            i = localLogicExpression1.GetDepth();

            if (i == 2)
            {
                var localLogicBranch = (LogicBranch) localLogicExpression1;
                LogicExpression[] arrayOfLogicExpression = localLogicBranch.Branches;

                if (arrayOfLogicExpression.Length == 1)
                {
                    localLogicExpression1 = arrayOfLogicExpression[0];
                    localLogicExpression1.SetParent(null, -1);

                    paramLogicDerivation.AddStep(localLogicExpression1, "Associativity");
                }
            }
        }

        private static void CarryOutIdempotency(LogicDerivation paramLogicDerivation)
        {
            LogicExpression localLogicExpression1 = paramLogicDerivation.Next;

            int i = 0;

            int j = 0;
            LogicExpression localLogicExpression2;
            while ((localLogicExpression2 = localLogicExpression1.GetSubExpression(2, j)) != null)
            {
                if (idempotency((LogicBranch) localLogicExpression2))
                {
                    i = 1;
                    continue;
                }
                j++;
            }

            if (i != 0)
            {
                if ((localLogicExpression1 is LogicBranch))
                {
                    LogicExpression[] arrayOfLogicExpression = ((LogicBranch) localLogicExpression1).Branches;

                    if (arrayOfLogicExpression.Length == 1)
                    {
                        localLogicExpression1 = arrayOfLogicExpression[0];
                        localLogicExpression1.SetParent(null, -1);
                    }
                }

                paramLogicDerivation.AddStep(localLogicExpression1, "Idempotency");
            }
        }

        private static void CarryOutBoolValues(LogicDerivation paramLogicDerivation)
        {
            var logicExpression = paramLogicDerivation.Next;

            int i = logicExpression.GetDepth();

            for (int m = 2; m <= i; m++)
            {
                int k = 0;
                int j = 0;
                LogicExpression localLogicExpression;
                while ((localLogicExpression = logicExpression.GetSubExpression(m, j)) != null)
                {
                    var localLogicBranch1 = (LogicBranch) localLogicExpression;
                    int n = GetBoolResolution(localLogicBranch1);
                    if (n == 31)
                    {
                        LogicExpression[] arrayOfLogicExpression = localLogicBranch1.Branches;
                        int i1 = arrayOfLogicExpression.Length;

                        i1 -= arrayOfLogicExpression.Count(t => (t is LogicValue));
                        
                        if (i1 == 1)
                        {
                            LogicExpression localObject2 = arrayOfLogicExpression.FirstOrDefault(t => (!(t is LogicValue)));

                            LogicBranch localLogicBranch2 = localLogicExpression.Parent;

                            if (localLogicBranch2 == null)
                            {
                                logicExpression = localObject2;
                                localObject2.SetParent(null, -1);
                            }
                            else
                            {
                                localLogicBranch2.SetBranch(localObject2, localLogicExpression.PositionInParent);
                            }
                        }
                        else
                        {
                            var localObject22 = new LogicExpression[i1];

                            int i4 = 0;

                            foreach (LogicExpression t in arrayOfLogicExpression)
                            {
                                if (!(t is LogicValue))
                                {
                                    localObject22[(i4++)] = t;
                                }
                            }
                            localLogicBranch1.Branches = localObject22;

                            j++;
                        }

                        k = 1;
                    }
                    else if ((n == 32) || (n == 33))
                    {
                        bool @bool = n == 32;
                        var localLogicValue = new LogicValue(@bool);

                        var localObject2 = localLogicBranch1.Parent;

                        if (localObject2 == null)
                        {
                            localLogicValue.SetParent(null, -1);
                            paramLogicDerivation.AddStep(localLogicValue, "Resolved bool values");
                            break;
                        }

                        (localObject2).SetBranch(localLogicValue, localLogicBranch1.PositionInParent);

                        k = 1;
                    }
                    else
                    {
                        j++;
                    }
                }

                if (k == 0)
                    continue;
                paramLogicDerivation.AddStep(logicExpression, "Removed redundant bool values");
                logicExpression = paramLogicDerivation.Next;
            }
        }



        private static int GetBoolResolution(LogicBranch paramLogicBranch)
        {
            LogicExpression[] arrayOfLogicExpression = paramLogicBranch.Branches;

            int i = 0;
            int j = 0;

            foreach (LogicExpression t in arrayOfLogicExpression)
            {
                if (!(t is LogicValue))
                    continue;
                bool @bool = ((LogicValue) t).EqualsTrue();

                if (@bool)
                    i = 1;
                else
                {
                    j = 1;
                }
                if ((i != 0) && (j != 0))
                {
                    break;
                }
            }
            if ((i == 0) && (j == 0))
            {
                return 30;
            }
            var k1 = paramLogicBranch.Operator;

            if (k1 == Operator.And)
            {
                if (j != 0)
                {
                    return 33;
                }
                return 31;
            }
            if (k1 == Operator.Or)
            {
                if (i != 0)
                {
                    return 32;
                }
                return 31;
            }

            Console.Error.WriteLine("Software Error: Operator other than AND, OR found when looking at bool values");

            return 30;
        }

        private static void ReplaceImpliesOperator(LogicBranch paramLogicBranch)
        {
            LogicExpression[] arrayOfLogicExpression = paramLogicBranch.Branches;

            arrayOfLogicExpression[0].Negated = !arrayOfLogicExpression[0].Negated;

            paramLogicBranch.Operator = Operator.Or;
        }

        private static void ReplaceBiimpliesOperator(LogicBranch paramLogicBranch)
        {
            LogicExpression[] arrayOfLogicExpression1 = paramLogicBranch.Branches;

            LogicExpression[] arrayOfLogicExpression2 = {
                                                            arrayOfLogicExpression1[0].CloneLogic(),
                                                            arrayOfLogicExpression1[1].CloneLogic()
                                                        };

            LogicExpression[] arrayOfLogicExpression3 = {
                                                            arrayOfLogicExpression1[0].CloneLogic(),
                                                            arrayOfLogicExpression1[1].CloneLogic()
                                                        };

            arrayOfLogicExpression3[0].Negated = !arrayOfLogicExpression3[0].Negated;
            arrayOfLogicExpression3[1].Negated = !arrayOfLogicExpression3[1].Negated;

            var localLogicBranch1 = new LogicBranch(0, false) {Branches = arrayOfLogicExpression2};

            var localLogicBranch2 = new LogicBranch(0, false) {Branches = arrayOfLogicExpression3};

            LogicExpression[] arrayOfLogicExpression4 = {localLogicBranch1, localLogicBranch2};

            paramLogicBranch.Branches = arrayOfLogicExpression4;
            paramLogicBranch.Operator = Operator.Or;
        }

        private static void ReplaceXorOperator(LogicBranch paramLogicBranch)
        {
            LogicExpression[] arrayOfLogicExpression1 = paramLogicBranch.Branches;

            LogicExpression[] arrayOfLogicExpression2 = {
                                                            arrayOfLogicExpression1[0].CloneLogic(),
                                                            arrayOfLogicExpression1[1].CloneLogic()
                                                        };

            arrayOfLogicExpression2[0].Negated = !arrayOfLogicExpression2[0].Negated;

            LogicExpression[] arrayOfLogicExpression3 = {
                                                            arrayOfLogicExpression1[0].CloneLogic(),
                                                            arrayOfLogicExpression1[1].CloneLogic()
                                                        };

            arrayOfLogicExpression3[1].Negated = !arrayOfLogicExpression3[1].Negated;

            var localLogicBranch1 = new LogicBranch(0, false) {Branches = arrayOfLogicExpression2};

            var localLogicBranch2 = new LogicBranch(0, false) {Branches = arrayOfLogicExpression3};

            LogicExpression[] arrayOfLogicExpression4 = {localLogicBranch1, localLogicBranch2};

            paramLogicBranch.Branches = arrayOfLogicExpression4;
            paramLogicBranch.Operator = Operator.Or;
        }

        private static bool DeMorgans(LogicBranch paramLogicBranch)
        {
            var i = paramLogicBranch.Operator;

            if (!paramLogicBranch.Negated)
            {
                return false;
            }
            LogicExpression[] arrayOfLogicExpression = paramLogicBranch.Branches;

            foreach (LogicExpression t in arrayOfLogicExpression)
            {
                t.Negated = !t.Negated;
            }
            Operator j = i == Operator.And ? Operator.Or : Operator.And;

            paramLogicBranch.Operator = j;
            paramLogicBranch.Negated = false;

            return true;
        }

        private static bool associativity(LogicBranch paramLogicBranch)
        {
            LogicExpression[] arrayOfLogicExpression1 = paramLogicBranch.Branches;

            int i = 0;

            for (int j = 0; j < arrayOfLogicExpression1.Length; j++)
            {
                if (!(arrayOfLogicExpression1[j] is LogicBranch))
                    continue;
                var localLogicBranch1 = (LogicBranch) arrayOfLogicExpression1[j];

                if ((localLogicBranch1.Operator != paramLogicBranch.Operator) ||
                    localLogicBranch1.Negated)
                {
                    continue;
                }
                i += localLogicBranch1.Branches.Length - 1;
            }

            if (i == 0)
            {
                return false;
            }

            var arrayOfLogicExpression2 = new LogicExpression[arrayOfLogicExpression1.Length + i];

            int k = 0;

            for (int m = 0; m < arrayOfLogicExpression1.Length; m++)
            {
                if ((arrayOfLogicExpression1[m] is LogicBranch))
                {
                    var localLogicBranch2 = (LogicBranch) arrayOfLogicExpression1[m];

                    if ((localLogicBranch2.Operator == paramLogicBranch.Operator) &&
                        (!localLogicBranch2.Negated))
                    {
                        LogicExpression[] arrayOfLogicExpression3 = localLogicBranch2.Branches;

                        for (int n = 0; n < arrayOfLogicExpression3.Length; n++)
                            arrayOfLogicExpression2[(k++)] = arrayOfLogicExpression3[n];
                    }
                    else
                    {
                        arrayOfLogicExpression2[(k++)] = arrayOfLogicExpression1[m];
                    }
                }
                else
                {
                    arrayOfLogicExpression2[(k++)] = arrayOfLogicExpression1[m];
                }
            }
            paramLogicBranch.Branches = arrayOfLogicExpression2;

            return true;
        }

        private static bool Distributivity(LogicBranch paramLogicBranch)
        {
            var i = paramLogicBranch.Operator;

            LogicExpression[] arrayOfLogicExpression1 = paramLogicBranch.Branches;
            var arrayOfLogicExpression = new LogicExpression[arrayOfLogicExpression1.Length][];

            int j = 0;
            int k = 1;
            LogicExpression[] arrayOfLogicExpression2;
            foreach (LogicExpression t in arrayOfLogicExpression1)
            {
                if ((t is LogicLeaf) || (t is LogicValue))
                {
                    arrayOfLogicExpression2 = new[] {t};
                    arrayOfLogicExpression[(j++)] = arrayOfLogicExpression2;
                }
                else
                {
                    arrayOfLogicExpression[j] = ((LogicBranch) t).Branches;
                    k *= arrayOfLogicExpression[j].Length;
                    j++;
                }
            }

            if (k*arrayOfLogicExpression1.Length == j)
            {
                return false;
            }
            var arrayOfInt = new int[arrayOfLogicExpression.Length];
            arrayOfInt[(arrayOfInt.Length - 1)] = -1;
            arrayOfLogicExpression2 = new LogicExpression[k];

            int n;
            for (n = 0; n < arrayOfLogicExpression2.Length; n++)
            {
                arrayOfInt[(arrayOfInt.Length - 1)] += 1;

                for (int i1 = arrayOfInt.Length - 1; i1 > 0; i1--)
                {
                    if (arrayOfInt[i1] != arrayOfLogicExpression[i1].Length)
                        break;
                    arrayOfInt[i1] = 0;
                    arrayOfInt[(i1 - 1)] += 1;
                }

                var arrayOfLogicExpression3 = new LogicExpression[arrayOfLogicExpression.Length];

                for (int i2 = 0; i2 < arrayOfLogicExpression3.Length; i2++)
                {
                    arrayOfLogicExpression3[i2] = arrayOfLogicExpression[i2][arrayOfInt[i2]];
                }
                if (arrayOfLogicExpression3.Length == 1)
                {
                    arrayOfLogicExpression2[n] = arrayOfLogicExpression3[0];
                }
                else
                {
                    var localLogicBranch = new LogicBranch(i, false)
                                               {
                                                   Branches = arrayOfLogicExpression3
                                               };
                    arrayOfLogicExpression2[n] = localLogicBranch;
                }
            }

            var nn = i == Operator.And ? Operator.Or : Operator.And;

            paramLogicBranch.Branches = arrayOfLogicExpression2;
            paramLogicBranch.Operator = nn;

            return true;
        }

        private static int Absorbtion(LogicBranch paramLogicBranch)
        {
            LogicExpression[] arrayOfLogicExpression1 = paramLogicBranch.Branches;

            int j = 0;
            int m;
            int n;
            for (int k = 0; k < arrayOfLogicExpression1.Length - 1; k++)
            {
                for (m = k + 1; (m < arrayOfLogicExpression1.Length) && (arrayOfLogicExpression1[k] != null); m++)
                {
                    if (arrayOfLogicExpression1[m] == null)
                        continue;
                    n = isAbsorbtion(arrayOfLogicExpression1[k], arrayOfLogicExpression1[m]);
                    switch (n)
                    {
                        case 21:
                            arrayOfLogicExpression1[m] = null;
                            j++;
                            break;
                        case 22:
                            arrayOfLogicExpression1[k] = null;
                            j++;
                            break;
                    }
                }
            }

            if (j > 0)
            {
                var arrayOfLogicExpression2 = new LogicExpression[arrayOfLogicExpression1.Length - j];

                m = 0;

                for (n = 0; n < arrayOfLogicExpression1.Length; n++)
                {
                    if (arrayOfLogicExpression1[n] != null)
                    {
                        arrayOfLogicExpression2[(m++)] = arrayOfLogicExpression1[n];
                    }
                }
                paramLogicBranch.Branches = arrayOfLogicExpression2;
            }

            return j;
        }

        private static int isAbsorbtion(LogicExpression paramLogicExpression1, LogicExpression paramLogicExpression2)
        {
            LogicExpression[] arrayOfLogicExpression1;
            if ((paramLogicExpression1 is LogicBranch))
            {
                arrayOfLogicExpression1 = ((LogicBranch) paramLogicExpression1).Branches;
            }
            else
            {
                arrayOfLogicExpression1 = new LogicExpression[1];
                arrayOfLogicExpression1[0] = paramLogicExpression1;
            }
            LogicExpression[] arrayOfLogicExpression2;
            if ((paramLogicExpression2 is LogicBranch))
            {
                arrayOfLogicExpression2 = ((LogicBranch) paramLogicExpression2).Branches;
            }
            else
            {
                arrayOfLogicExpression2 = new LogicExpression[1];
                arrayOfLogicExpression2[0] = paramLogicExpression2;
            }

            int i = arrayOfLogicExpression1.Length < arrayOfLogicExpression2.Length ? 1 : 0;
            LogicExpression[] arrayOfLogicExpression3;
            LogicExpression[] arrayOfLogicExpression4;
            if (i != 0)
            {
                arrayOfLogicExpression3 = arrayOfLogicExpression1;
                arrayOfLogicExpression4 = arrayOfLogicExpression2;
            }
            else
            {
                arrayOfLogicExpression3 = arrayOfLogicExpression2;
                arrayOfLogicExpression4 = arrayOfLogicExpression1;
            }

            foreach (LogicExpression t1 in arrayOfLogicExpression3)
            {
                int j = 0;

                if (arrayOfLogicExpression4.Any(t => t1.Equals(t)))
                {
                    j = 1;
                }

                if (j == 0)
                {
                    return 20;
                }
            }
            return i != 0 ? 21 : 22;
        }

        private static bool idempotency(LogicBranch paramLogicBranch)
        {
            var i = paramLogicBranch.Operator;

            LogicExpression[] arrayOfLogicExpression = paramLogicBranch.Branches;

            int j = arrayOfLogicExpression.Length;

            for (int k = 0; k < arrayOfLogicExpression.Length; k++)
            {
                if ((arrayOfLogicExpression[k] == null) || (!(arrayOfLogicExpression[k] is LogicLeaf)))
                    continue;
                var localLogicLeaf1 = (LogicLeaf) arrayOfLogicExpression[k];

                for (int n = k + 1; n < arrayOfLogicExpression.Length; n++)
                {
                    if ((arrayOfLogicExpression[n] == null) || (!(arrayOfLogicExpression[n] is LogicLeaf)))
                        continue;
                    var localLogicLeaf2 = (LogicLeaf) arrayOfLogicExpression[n];

                    if (!localLogicLeaf1.Name.Equals(localLogicLeaf2.Name))
                        continue;
                    if (arrayOfLogicExpression[k].Negated != arrayOfLogicExpression[n].Negated)
                    {
                        switch (i)
                        {
                            case Operator.And:
                                arrayOfLogicExpression[k] = new LogicValue(false);
                                break;
                            case Operator.Or:
                                arrayOfLogicExpression[k] = new LogicValue(true);
                                break;
                            default:
                                Console.Error.WriteLine("Software Error: Unimplemented operator: " + i);
                                break;
                        }
                    }

                    arrayOfLogicExpression[n] = null;
                    j--;
                }
            }

            if (j == arrayOfLogicExpression.Length)
                return false;
            if (j == 1)
            {
                var localObject1 = paramLogicBranch.Parent;

                if (localObject1 != null)
                {
                    (localObject1).SetBranch(arrayOfLogicExpression[0], paramLogicBranch.PositionInParent);
                }
                return true;
            }

            var localObject = new LogicExpression[j];
            j = 0;

            foreach (LogicExpression t in arrayOfLogicExpression.Where(t => t != null))
            {
                localObject[j++] = t;
            }
            paramLogicBranch.Branches = localObject;

            return true;
        }
    }
}

/* Location:           D:\Downloads\boolLogicApplet.jar
 * Qualified Name:     com.izyt.boolLogic.LogicHandler
 * JD-Core Version:    0.6.0
 */