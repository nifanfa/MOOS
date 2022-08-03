using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Method)]
    public class CompilationRelaxationsAttribute : Attribute
    {
        public CompilationRelaxationsAttribute(int relaxations)
        {
        }
    }
}
