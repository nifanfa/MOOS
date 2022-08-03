using System;
using System.Collections.Generic;
using System.Text;

namespace System.Diagnostics
{
    public enum DebuggerBrowsableState
    {
        Never = 0,

        //Expanded is not supported in this release
        //Expanded = 1,
        Collapsed = 2,

        RootHidden = 3
    }

    // the one currently supported with the csee.dat
    // (mcee.dat, autoexp.dat) file.
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DebuggerBrowsableAttribute : Attribute
    {
        private readonly DebuggerBrowsableState state;

        public DebuggerBrowsableAttribute(DebuggerBrowsableState state)
        {
            if (state < DebuggerBrowsableState.Never || state > DebuggerBrowsableState.RootHidden)
                throw new ArgumentOutOfRangeException("state");

            this.state = state;
        }

        public DebuggerBrowsableState State
        {
            get { return state; }
        }
    }
}
