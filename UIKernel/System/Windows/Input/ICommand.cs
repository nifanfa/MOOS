using System;
using System.Collections.Generic;


namespace System.Windows.Input
{
    public class ICommand
    {
        public Action<object> Execute { set; get; }

        public ICommand(Action<object> action)
        {
            Execute = action;
        }
    }
}
