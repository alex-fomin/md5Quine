#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace Derivations
{
    public class LogicBranch : LogicExpression
    {
        private LogicExpression[] _logicBranches;

        public LogicBranch(Operator @operator, bool negated)
            : base(negated)
        {
            Operator = @operator;
        }

        public Operator Operator { get; set; }

        public override bool Evaluate(Dictionary<String, Mutablebool> paramHashMap)
        {
            int i;
            bool left = _logicBranches[0].Evaluate(paramHashMap);
            bool right = _logicBranches[1].Evaluate(paramHashMap);
            switch (Operator)
            {
                case Operator.And:
                    for (i = 0; i < _logicBranches.Length; i++)
                        if (!_logicBranches[i].Evaluate(paramHashMap))
                            return false;
                    return true;
                case Operator.Or:
                    for (i = 0; i < _logicBranches.Length; i++)
                        if (_logicBranches[i].Evaluate(paramHashMap))
                            return true;
                    return false;
                case Operator.Implies:
                    return (!left) || right;
                case Operator.Biimplies:
                    return left == right;
                case Operator.Xor:
                    return left != right;
            }
            Console.Error.WriteLine("Software Error: Unknown operator");
            return false;
        }

        public override void GetLeafs(HashSet<String> paramLinkedHashSet)
        {
            foreach (LogicExpression t in _logicBranches)
                t.GetLeafs(paramLinkedHashSet);
        }

        public override int GetDepth(int paramInt)
        {
            paramInt++;

            int i = paramInt;

            foreach (LogicExpression t in _logicBranches)
            {
                int j;
                if ((j = t.GetDepth(paramInt)) > i)
                {
                    i = j;
                }
            }
            return i;
        }

        public override int GetNumberOfElements(int paramInt)
        {
            return _logicBranches.Aggregate(paramInt, (current, t) => t.GetNumberOfElements(current));
        }

        public bool IsNormalForm()
        {
            if (Negated)
            {
                return false;
            }

            if ((Operator != Operator.And) && (Operator != Operator.Or))
            {
                return false;
            }

            return _logicBranches.All(t => t.GetLogicalForm() != 0);
        }

        public Form IsCnforDNF()
        {
            if (_logicBranches.OfType<LogicBranch>().Any(t => (t).Operator == Operator))
            {
                return Form.NegationNormalForm;
            }

            return Operator == Operator.And ? Form.CNF : Form.DNF;
        }

        public override LogicExpression GetSubExpression(int paramInt1, int paramInt2, MutableInteger paramMutableInteger)
        {
            int i = GetDepth();

            if ((i == 1) && (paramInt1 == 1))
            {
                if (paramMutableInteger.Value > paramInt2)
                {
                    return this;
                }

                paramMutableInteger.Value += 1;
                return null;
            }

            if (paramInt1 == i)
            {
                if (paramMutableInteger.Value > paramInt2)
                {
                    return this;
                }

                paramMutableInteger.Value += 1;
                return null;
            }

            return _logicBranches.Select(t => t.GetSubExpression(paramInt1, paramInt2, paramMutableInteger)).FirstOrDefault(localLogicExpression => localLogicExpression != null);
        }

        public LogicExpression[] Branches
        {
            get { return _logicBranches; }
            set
            {
                _logicBranches = value;

                if ((value.Length != 2) && (Operator != Operator.And) && (Operator != Operator.Or))
                {
                    Console.Error.WriteLine("Software Error: Invalid number of operators");
                }

                for (int i = 0; i < value.Length; i++)
                    value[i].SetParent(this, i);
            }
        }

        public void SetBranch(LogicExpression paramLogicExpression, int position)
        {
            _logicBranches[position] = paramLogicExpression;
            paramLogicExpression.SetParent(this, position);
        }

        public override bool HasNegatedBranches()
        {
            return Negated || _logicBranches.Any(t => t.HasNegatedBranches());
        }

        public override void ToString(StringBuilder paramStringBuilder, LogicSyntax paramInt1, NegationSyntax paramInt2)
        {
            if (Negated)
            {
                string value = NegationSyntaxer.GetNegation(paramInt2);
                paramStringBuilder.Append(value);
            }
            if (Parent != null)
            {
                paramStringBuilder.Append("(");
            }
            for (int i = 0; i < _logicBranches.Length; i++)
            {
                if (i > 0)
                {
                    paramStringBuilder.Append(" ");
                    paramStringBuilder.Append(LogicSyntaxer.GetOperatorString(paramInt1, Operator));
                    paramStringBuilder.Append(" ");
                }

                _logicBranches[i].ToString(paramStringBuilder, paramInt1, paramInt2);
            }

            if (Parent != null)
            {
                paramStringBuilder.Append(")");
            }
            if ((Negated) && (paramInt2 == NegationSyntax.After))
                paramStringBuilder.Append("'");
        }

        public override LogicExpression CloneLogic()
        {
            var localLogicBranch = new LogicBranch(Operator, Negated);

            var arrayOfLogicExpression = new LogicExpression[_logicBranches.Length];

            for (int i = 0; i < _logicBranches.Length; i++)
            {
                arrayOfLogicExpression[i] = _logicBranches[i].CloneLogic();
            }
            localLogicBranch.Branches = arrayOfLogicExpression;
            return localLogicBranch;
        }


    }
}