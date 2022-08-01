using System;
using System.Collections.Generic;


namespace System.Windows.Controls
{
    public class ColumnDefinitionCollection : List<ColumnDefinition>
    {
        public GridLength Width { set; get; }

        public ColumnDefinitionCollection() : base()
        {
        }
    }
}
