#region

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Derivations
{
    public class LogicLeaf : LogicExpression
    {
        public LogicLeaf(String paramString) : this(paramString, false)
        {
        }

        public LogicLeaf(String paramString, bool negated) : base(negated)
        {
            Name = paramString;
        }

        public override bool Evaluate(Dictionary<String, Mutablebool> paramHashMap)
        {
            bool @bool = (paramHashMap[Name])._value;

            return Negated ? !@bool : @bool;
        }

        public override void GetLeafs(HashSet<String> paramLinkedHashSet)
        {
            paramLinkedHashSet.Add(Name);
        }

        public string Name { get; private set; }

        public override void ToString(StringBuilder paramStringBuilder, LogicSyntax paramInt1, NegationSyntax paramInt2)
        {
            if (Negated)
            {
                string value = NegationSyntaxer.GetNegatedString(paramInt2, Name);
                paramStringBuilder.Append(value);
            }
            else
                paramStringBuilder.Append(Name);
        }

        public override LogicExpression CloneLogic()
        {
            return new LogicLeaf(Name, Negated);
        }

        public override bool Equals(LogicExpression paramLogicExpression)
        {
            return ((paramLogicExpression is LogicLeaf)) && ((LogicLeaf) paramLogicExpression).Name.Equals(Name) &&
                   (paramLogicExpression.Negated == Negated);
        }
    }
}