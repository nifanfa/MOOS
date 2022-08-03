using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections
{
    public interface IDictionaryEnumerator : IEnumerator
    {
        // Returns the key of the current element of the enumeration. The returned
        // value is undefined before the first call to GetNext and following
        // a call to GetNext that returned false. Multiple calls to
        // GetKey with no intervening calls to GetNext will return
        // the same object.
        // 
        Object Key
        {
            get;
        }

        // Returns the value of the current element of the enumeration. The
        // returned value is undefined before the first call to GetNext and
        // following a call to GetNext that returned false. Multiple calls
        // to GetValue with no intervening calls to GetNext will
        // return the same object.
        // 
        Object Value
        {
            get;
        }

        // GetBlock will copy dictionary values into the given Array.  It will either
        // fill up the array, or if there aren't enough elements, it will
        // copy as much as possible into the Array.  The number of elements
        // copied is returned.
        // 
        DictionaryEntry Entry
        {
            get;
        }
    }
}
