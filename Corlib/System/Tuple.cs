namespace System
{
    public class Tuple<T1, T2> //: IStructuralEquatable, IStructuralComparable, IComparable, ITupleInternal, ITuple
    {
        private readonly T1 m_Item1; // Do not rename (binary serialization)

        private readonly T2 m_Item2; // Do not rename (binary serialization)

        public T1 Item1 { get { return m_Item1; } }

        public T2 Item2 { get { return m_Item2; } }

        public Tuple(T1 item1, T2 item2)
        {
            m_Item1 = item1;
            m_Item2 = item2;
        }
    }
}