#region

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Derivations
{
    public class LogicDerivation
    {
        private readonly String _initialText;
        private readonly List<LogicStep> _steps;
        private int _cnfNumberOfElements;
        private int _dnfNumberOfElements;

        public LogicDerivation(String paramString, LogicExpression paramLogicExpression)
        {
            _initialText = paramString;

            _steps = new List<LogicStep>();

            AddStep(paramLogicExpression, "Initial parsed expression");
        }

        public void AddStep(LogicExpression paramLogicExpression, String paramString)
        {
            var i = paramLogicExpression.GetLogicalForm();

            Next = paramLogicExpression.CloneLogic();

            var localLogicStep = new LogicStep
                                     {
                                         Expression = paramLogicExpression, 
                                         Comment = paramString, 
                                         Form = i
                                     };

            _steps.Add(localLogicStep);

            int j = paramLogicExpression.GetNumberOfElements();

            switch (i)
            {
                case Form.CNF:
                    if ((CNF == null) || (j < _cnfNumberOfElements))
                    {
                        CNF = paramLogicExpression;
                        _cnfNumberOfElements = j;
                    }
                    break;
                case Form.DNF:
                    if ((DNF == null) || (j < _dnfNumberOfElements))
                    {
                        DNF = paramLogicExpression;
                        _dnfNumberOfElements = j;
                    }
                    break;
                case Form.CNF_DNF:
                    if ((CNF == null) || (j < _cnfNumberOfElements))
                    {
                        CNF = paramLogicExpression;
                        _cnfNumberOfElements = j;
                    }
                    if ((DNF == null) || (j < _dnfNumberOfElements))
                    {
                        DNF = paramLogicExpression;
                        _dnfNumberOfElements = j;
                    }
                    break;
            }
        }

        public LogicExpression Next { get; private set; }

        public LogicExpression CNF { get; private set; }

        public LogicExpression DNF { get; private set; }

        public bool CNFAndDNF
        {
            get { return (CNF != null) && (DNF != null); }
        }

        public LogicStep[] Steps
        {
            get { return _steps.ToArray(); }
        }

        public void PrintTrace(LogicSyntax paramInt1, NegationSyntax paramInt2)
        {
            int i = _initialText.Length;
            var localStringBuilder = new StringBuilder();

            for (int j = 0; j < i; j++)
            {
                localStringBuilder.Append(" ");
            }
            String str = localStringBuilder.ToString();
            int k = 1;

            foreach (LogicStep localLogicStep in _steps)
            {
                Console.WriteLine(
                    new StringBuilder().Append(k != 0 ? _initialText : str).Append(" |=| ").Append(
                        localLogicStep.Expression.ToString(paramInt1, paramInt2)).Append(" - ").Append(
                            localLogicStep.Comment).Append(" - ").Append(LogicHandler.GetFormName(localLogicStep.Form)).
                        ToString());

                k = 0;
            }

            Console.WriteLine(new StringBuilder().Append("CNF: ").Append(CNF.ToString(paramInt1, paramInt2)).ToString());
            Console.WriteLine(new StringBuilder().Append("DNF: ").Append(DNF.ToString(paramInt1, paramInt2)).ToString());
        }
    }
}