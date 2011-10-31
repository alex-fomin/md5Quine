#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace Derivations
{
    public abstract class LogicExpression
    {
        protected LogicExpression(bool negated)
        {
            PositionInParent = -1;
            Negated = negated;
        }

        public bool Negated { get; set; }

        public void SetParent(LogicBranch parent, int positionInParent)
        {
            Parent = parent;
            PositionInParent = positionInParent;
        }

        public LogicBranch Parent { get; protected set; }

        public int PositionInParent { get; private set; }

        public int GetDepth()
        {
            return GetDepth(0);
        }

        public Form GetLogicalForm()
        {
            if ((this is LogicBranch))
            {
                if (!((LogicBranch) this).IsNormalForm())
                {
                    return Form.None;
                }
                int i = GetDepth();

                if (i == 2)
                    return Form.CNF_DNF;
                if (i == 3)
                {
                    return ((LogicBranch) this).IsCnforDNF();
                }
                return Form.NegationNormalForm;
            }

            return Form.CNF_DNF;
        }

        public String[] GetVariableNames()
        {
            var localLinkedHashSet = new HashSet<string>();
            GetLeafs(localLinkedHashSet);

            return localLinkedHashSet.ToArray();
        }

        public int GetNumberOfElements()
        {
            return GetNumberOfElements(0);
        }

        public LogicExpression GetSubExpression(int paramInt1, int paramInt2)
        {
            return GetSubExpression(paramInt1, paramInt2, new MutableInteger(1));
        }

        public virtual LogicExpression GetSubExpression(int paramInt1, int paramInt2, MutableInteger paramMutableInteger)
        {
            if ((paramInt1 == 1) && (GetDepth() == 1))
            {
                if (paramMutableInteger.Value > paramInt2)
                {
                    return this;
                }

                paramMutableInteger.Value += 1;
                return null;
            }

            return null;
        }

        public override String ToString()
        {
            return ToString( LogicSyntax.DotPlus, NegationSyntax.After);
        }

        public String ToString(LogicSyntax logicSyntax, NegationSyntax negationSyntax)
        {
            var localStringBuilder = new StringBuilder();

            ToString(localStringBuilder, logicSyntax, negationSyntax);

            return localStringBuilder.ToString();
        }

        public virtual bool Equals(LogicExpression paramLogicExpression)
        {
            return false;
        }

        public virtual void GetLeafs(HashSet<String> paramLinkedHashSet)
        {
        }

        public virtual int GetDepth(int paramInt)
        {
            return paramInt + 1;
        }

        public virtual int GetNumberOfElements(int paramInt)
        {
            return paramInt + 1;
        }

        public virtual bool HasNegatedBranches()
        {
            return false;
        }

        public abstract void ToString(StringBuilder paramStringBuilder, LogicSyntax paramInt1, NegationSyntax paramInt2);

        public abstract bool Evaluate(Dictionary<String, Mutablebool> paramHashMap);

        public abstract LogicExpression CloneLogic();
    }
}