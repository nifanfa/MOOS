using System;
using System.Collections.Generic;
using System.Text;

namespace System.Diagnostics.Contracts
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ConditionalAttribute : Attribute
    {
        private readonly string conditionString;

        public string ConditionString
        {
            get { return conditionString; }
        }

        public ConditionalAttribute(string conditionString)
        {
            this.conditionString = ConditionString;
        }
    }
}
