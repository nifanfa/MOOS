using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace System.Collections
{
    [Serializable]
    internal class CompatibleComparer : IEqualityComparer
    {
        readonly IComparer _comparer;
#pragma warning disable 618
        readonly IHashCodeProvider _hcp;

        internal CompatibleComparer(IComparer comparer, IHashCodeProvider hashCodeProvider)
        {
            _comparer = comparer;
            _hcp = hashCodeProvider;
        }
#pragma warning restore 618

        public int Compare(Object a, Object b)
        {
            if (a == b) return 0;
            if (a == null) return -1;
            if (b == null) return 1;
            if (_comparer != null)
                return _comparer.Compare(a, b);
            IComparable ia = a as IComparable;
            if (ia != null)
                return ia.CompareTo(b);

            throw new ArgumentException("Argument_ImplementIComparable");
        }

        public new bool Equals(Object a, Object b)
        {
            return Compare(a, b) == 0;
        }

        public int GetHashCode(Object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            Contract.EndContractBlock();

            if (_hcp != null)
                return _hcp.GetHashCode(obj);
            return obj.GetHashCode();
        }

        // These are helpers for the Hashtable to query the IKeyComparer infrastructure.
        internal IComparer Comparer
        {
            get
            {
                return _comparer;
            }
        }

        // These are helpers for the Hashtable to query the IKeyComparer infrastructure.
        internal IHashCodeProvider HashCodeProvider
        {
            get
            {
                return _hcp;
            }
        }
    }
}
