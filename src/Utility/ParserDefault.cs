using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniverseLib.Utility
{
    public sealed class ParserDefault : Parser
    {
        public override bool CanParse(Type type)
            => ParseUtility.CanParse(type);

        public override string ToStringForInput(object obj, Type type)
            => ParseUtility.ToStringForInput(obj, type);

        public override bool TryParse(string input, Type type, out object obj, out Exception parseException)
            => ParseUtility.TryParse(input, type, out obj, out parseException);
    }
}
