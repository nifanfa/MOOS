using System;
using System.Collections.Generic;

namespace System.Windows.Controls
{
    public class GridCollection
    {
        public Position Position { set; get; }
        public int Row { set; get; }
        public int Column { set; get; }

        public GridCollection(int row, int column)
        {
            Position = new Position();
            Row = row;
            Column = column;
        }

        public override string ToString()
        {
            return $"X: {Position.X}, Y: {Position.Y}, Width: {Position.Width}, Height: {Position.Height}";
        }
    }
}
