using System;
using System.Collections.Generic;


namespace System.Windows.Controls
{
    public class RowDefinitionCollection : List<RowDefinition>
    {
        public GridLength Height { set; get; }

        public RowDefinitionCollection() 
        {
           new List<RowDefinition>();
        }

    }
}
