using System.Runtime.InteropServices;

    public struct ValueTuple<T1, T2> //: IEquatable<ValueTuple<T1, T2>>
    {
        //readonly static EqualityComparer<T1> Comparer1 = EqualityComparer<T1>.Default;
        //readonly static EqualityComparer<T2> Comparer2 = EqualityComparer<T2>.Default;

        public ValueTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

        //public bool Equals(ValueTuple<T1, T2> other) => Comparer1.Equals(Item1, other.Item1) && Comparer2.Equals(Item2, other.Item2);

        public override bool Equals(object obj)
        {
            if (obj is ValueTuple<T1, T2>)
            {
                var other = (ValueTuple<T1, T2>)obj;
                return Equals(other);
            }

            return false;
        }

        //public override int GetHashCode() => Hash.Combine(Comparer1.GetHashCode(Item1), Comparer2.GetHashCode(Item2));

        public override int GetHashCode()
        {
            return Item1.GetHashCode() ^ Item2.GetHashCode();
        }

        public static bool operator ==(ValueTuple<T1, T2> left, ValueTuple<T1, T2> right) => left.Equals(right);

        public static bool operator !=(ValueTuple<T1, T2> left, ValueTuple<T1, T2> right) => !left.Equals(right);
    }
