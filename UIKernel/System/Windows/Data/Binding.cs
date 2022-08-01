using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace System.Windows.Data
{
    public class Binding
    {
        public ICommand Source { set; get; }

        public Binding(string path = "")
        {
        }
    }
}
