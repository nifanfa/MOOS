using System;
using System.Collections.Generic;


namespace System.Windows.Controls
{
    public class RowDefinition 
    {
        public Position Position { set; get; }
        public int ActualHeight { get; }
        public int MaxHeight { get; set; }
        public int MinHeight { get; set; }
        public int Offset { get; }
        public GridLength Height { set; get; }

        public RowDefinition()
        {
            Position = new Position();
            Height = new GridLength(1, GridUnitType.Star);
        }

        public override string ToString()
        {
            return $"X: {Position.X}, Y: {Position.Y}, Width: {Position.Width}, Height: {Position.Height}";
        }
    }
}
