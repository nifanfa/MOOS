namespace System
{
	public interface ITuple
	{
		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int Length { get; }

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
#nullable enable
		object? this[int index] { get; }
#nullable disable
	}

	/// <summary>
	/// Helper so we can call some tuple methods recursively without knowing the underlying types.
	/// </summary>
	internal interface ITupleInternal : ITuple
	{
		//string ToString();
		//int GetHashCode();
	}

	public static class Tuple
	{
		public static Tuple<T1> Create<T1>(T1 item1)
		{
			return new Tuple<T1>(item1);
		}

		public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
		{
			return new Tuple<T1, T2>(item1, item2);
		}

		public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
		{
			return new Tuple<T1, T2, T3>(item1, item2, item3);
		}

		public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
		}

		public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
		}

		public static Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
		}

		public static Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
		}

		public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>>(item1, item2, item3, item4, item5, item6, item7, new Tuple<T8>(item8));
		}
	}
	public class Tuple<T1> : ITupleInternal, ITuple
	{
		private readonly T1 m_Item1;

		public T1 Item1 => m_Item1;

		public Tuple(T1 item1)
		{
			m_Item1 = item1;
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 1;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>

		object ITuple.this[int index] => index != 0 ? null : Item1;
	}
	public class Tuple<T1, T2> : ITupleInternal, ITuple
	{

		private readonly T1 m_Item1;
		private readonly T2 m_Item2;

		public T1 Item1 => m_Item1;
		public T2 Item2 => m_Item2;

		public Tuple(T1 item1, T2 item2)
		{
			m_Item1 = item1;
			m_Item2 = item2;
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 2;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
		object ITuple.this[int index] => index switch
		{
			0 => Item1,
			1 => Item2,
			_ => null,
		};
	}
	public class Tuple<T1, T2, T3> : ITupleInternal, ITuple
	{

		private readonly T1 m_Item1;
		private readonly T2 m_Item2;
		private readonly T3 m_Item3;

		public T1 Item1 => m_Item1;
		public T2 Item2 => m_Item2;
		public T3 Item3 => m_Item3;

		public Tuple(T1 item1, T2 item2, T3 item3)
		{
			m_Item1 = item1;
			m_Item2 = item2;
			m_Item3 = item3;
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 3;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
		object ITuple.this[int index] => index switch
		{
			0 => Item1,
			1 => Item2,
			2 => Item3,
			_ => null,

		};
	}
	public class Tuple<T1, T2, T3, T4> : ITupleInternal, ITuple
	{

		private readonly T1 m_Item1;
		private readonly T2 m_Item2;
		private readonly T3 m_Item3;
		private readonly T4 m_Item4;

		public T1 Item1 => m_Item1;
		public T2 Item2 => m_Item2;
		public T3 Item3 => m_Item3;
		public T4 Item4 => m_Item4;

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			m_Item1 = item1;
			m_Item2 = item2;
			m_Item3 = item3;
			m_Item4 = item4;
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 4;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
		object ITuple.this[int index] => index switch
		{
			0 => Item1,
			1 => Item2,
			2 => Item3,
			3 => Item4,
			_ => null,
		};
	}
	public class Tuple<T1, T2, T3, T4, T5> : ITupleInternal, ITuple
	{

		private readonly T1 m_Item1;
		private readonly T2 m_Item2;
		private readonly T3 m_Item3;
		private readonly T4 m_Item4;
		private readonly T5 m_Item5;

		public T1 Item1 => m_Item1;
		public T2 Item2 => m_Item2;
		public T3 Item3 => m_Item3;
		public T4 Item4 => m_Item4;
		public T5 Item5 => m_Item5;

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			m_Item1 = item1;
			m_Item2 = item2;
			m_Item3 = item3;
			m_Item4 = item4;
			m_Item5 = item5;
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 5;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
		object ITuple.this[int index] => index switch
		{
			0 => Item1,
			1 => Item2,
			2 => Item3,
			3 => Item4,
			4 => Item5,
			_ => null,
		};
	}
	public class Tuple<T1, T2, T3, T4, T5, T6> : ITupleInternal, ITuple
	{

		private readonly T1 m_Item1;
		private readonly T2 m_Item2;
		private readonly T3 m_Item3;
		private readonly T4 m_Item4;
		private readonly T5 m_Item5;
		private readonly T6 m_Item6;

		public T1 Item1 => m_Item1;
		public T2 Item2 => m_Item2;
		public T3 Item3 => m_Item3;
		public T4 Item4 => m_Item4;
		public T5 Item5 => m_Item5;
		public T6 Item6 => m_Item6;

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			m_Item1 = item1;
			m_Item2 = item2;
			m_Item3 = item3;
			m_Item4 = item4;
			m_Item5 = item5;
			m_Item6 = item6;
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 6;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
		object ITuple.this[int index] => index switch
		{
			0 => Item1,
			1 => Item2,
			2 => Item3,
			3 => Item4,
			4 => Item5,
			5 => Item6,
			_ => null
		};
	}

	public class Tuple<T1, T2, T3, T4, T5, T6, T7> : ITupleInternal, ITuple
	{

		private readonly T1 m_Item1;
		private readonly T2 m_Item2;
		private readonly T3 m_Item3;
		private readonly T4 m_Item4;
		private readonly T5 m_Item5;
		private readonly T6 m_Item6;
		private readonly T7 m_Item7;

		public T1 Item1 => m_Item1;
		public T2 Item2 => m_Item2;
		public T3 Item3 => m_Item3;
		public T4 Item4 => m_Item4;
		public T5 Item5 => m_Item5;
		public T6 Item6 => m_Item6;
		public T7 Item7 => m_Item7;

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			m_Item1 = item1;
			m_Item2 = item2;
			m_Item3 = item3;
			m_Item4 = item4;
			m_Item5 = item5;
			m_Item6 = item6;
			m_Item7 = item7;
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 7;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
		object ITuple.this[int index] => index switch
		{
			0 => Item1,
			1 => Item2,
			2 => Item3,
			3 => Item4,
			4 => Item5,
			5 => Item6,
			6 => Item7,
			_ => null,
		};
	}

	public class Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> : ITupleInternal, ITuple
	{

		private readonly T1 m_Item1;
		private readonly T2 m_Item2;
		private readonly T3 m_Item3;
		private readonly T4 m_Item4;
		private readonly T5 m_Item5;
		private readonly T6 m_Item6;
		private readonly T7 m_Item7;
		private readonly TRest m_Rest;

		public T1 Item1 => m_Item1;
		public T2 Item2 => m_Item2;
		public T3 Item3 => m_Item3;
		public T4 Item4 => m_Item4;
		public T5 Item5 => m_Item5;
		public T6 Item6 => m_Item6;
		public T7 Item7 => m_Item7;
		public TRest Rest => m_Rest;

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
		{
			if (rest is not ITupleInternal)
			{
				return;
			}

			m_Item1 = item1;
			m_Item2 = item2;
			m_Item3 = item3;
			m_Item4 = item4;
			m_Item5 = item5;
			m_Item6 = item6;
			m_Item7 = item7;
			m_Rest = rest;
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 7 + ((ITupleInternal)Rest).Length;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
		object ITuple.this[int index] => index switch
		{
			0 => Item1,
			1 => Item2,
			2 => Item3,
			3 => Item4,
			4 => Item5,
			5 => Item6,
			6 => Item7,
			_ => ((ITupleInternal)Rest)[index - 7],
		};
	}
}