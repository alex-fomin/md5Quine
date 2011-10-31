#region

using System;

#endregion

namespace Derivations
{
    public class Mutablebool
    {
        public const int TrueFalse = 1;
        public const int OneZero = 2;
        public static int _boolSyntax = 1;
        public bool _value;

        public Mutablebool(bool parambool)
        {
            _value = parambool;
        }

        public override String ToString()
        {
            switch (_boolSyntax)
            {
                case TrueFalse:
                    return _value ? "true" : "false";
                case OneZero:
                    return _value ? "1" : "0";
            }
            Console.Error.WriteLine("Software Error: unknown boolSyntax: " + _boolSyntax);
            return _value ? "true" : "false";
        }

        public Mutablebool Clone()
        {
            return new Mutablebool(_value);
        }
    }
}