using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System
{
    [Serializable]
    public sealed class CharEnumerator : IEnumerator, ICloneable, IEnumerator<char>, IDisposable
    {
        private string str;
        private int currentPosition;
        private char currentElement;

        internal CharEnumerator(string str)
        {
            this.str = str;
            currentPosition = -1;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public bool MoveNext()
        {
            if (currentPosition < (str.Length - 1))
            {
                currentPosition++;
                currentElement = str[currentPosition];
                return true;
            }
            else
                currentPosition = str.Length;
            return false;
        }

        public void Dispose()
        {
            if (str != null)
                currentPosition = str.Length;
            str = null;
        }

        /// <internalonly/>
        object IEnumerator.Current
        {
            get
            {
                if (currentPosition == -1)
                    throw new InvalidOperationException("Enumeration has not started.");
                if (currentPosition >= str.Length)
                    throw new InvalidOperationException("Enumeration has already ended.");

                return currentElement;
            }
        }

        public char Current
        {
            get
            {
                if (currentPosition == -1)
                    throw new InvalidOperationException("Enumeration has not started.");
                if (currentPosition >= str.Length)
                    throw new InvalidOperationException("Enumeration has already ended.");
                return currentElement;
            }
        }

        public void Reset()
        {
            currentElement = (char)0;
            currentPosition = -1;
        }
    }
}
