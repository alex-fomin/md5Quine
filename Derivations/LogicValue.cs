#region

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Derivations
{
    public class LogicValue : LogicExpression
    {
        public LogicValue(bool value) : base(false)
        {
            Value = value;
        }

        public LogicValue(bool value, bool negated) : base(negated)
        {
            Value = value;
        }

        public bool Value { get; set; }

        public bool EqualsTrue()
        {
            return Negated != Value;
        }

        public override void ToString(StringBuilder paramStringBuilder, LogicSyntax paramInt1, NegationSyntax paramInt2)
        {
            if (Negated)
            {
                if (paramInt2 == NegationSyntax.Written)
                    paramStringBuilder.Append("NOT " + Value);
                else if (paramInt2 == NegationSyntax.Before)
                    paramStringBuilder.Append("~" + Value);
                else if (paramInt2 == NegationSyntax.After)
                    paramStringBuilder.Append(Value + "'");
                else
                {
                    Console.Error.WriteLine("Software Error: Unknown negationSyntax passed: " + paramInt2);
                }
            }
            else
                paramStringBuilder.Append(Value);
        }

        public override bool Evaluate(Dictionary<String, Mutablebool> paramHashMap)
        {
            return Negated != Value;
        }

        public override LogicExpression CloneLogic()
        {
            return new LogicValue(Value, Negated);
        }

        public override bool Equals(LogicExpression paramLogicExpression)
        {
            return ((paramLogicExpression is LogicValue)) &&
                   (((LogicValue) paramLogicExpression).EqualsTrue() == EqualsTrue());
        }
    }
}
