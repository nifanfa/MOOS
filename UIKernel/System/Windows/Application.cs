using System;
using System.Collections.Generic;

namespace System.Windows
{
    public class Application
    {
        public Window MainWindow { get; set; }

        public Application()
        {

        }

        public virtual void OnStartup(EventArgs e)
        {

        }

        public virtual void OnActivated(EventArgs e)
        {

        }

        public virtual void OnDeactivated(EventArgs e)
        { 
        
        }

        public virtual void OnExit(EventArgs e)
        {
            
        }
    }
}
