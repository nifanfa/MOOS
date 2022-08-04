using System.Runtime.InteropServices;

namespace System
{
	/// <summary>
	/// Helper so we can call some tuple methods recursively without knowing the underlying types.
	/// </summary>
	internal interface IValueTupleInternal : ITuple
	{
		//int GetHashCode(IEqualityComparer comparer);
		//string ToStringEnd();
	}

	/// <summary>
	/// The ValueTuple types (from arity 0 to 8) comprise the runtime implementation that underlies tuples in C# and struct tuples in F#.
	/// Aside from created via language syntax, they are most easily created via the ValueTuple.Create factory methods.
	/// The System.ValueTuple types differ from the System.Tuple types in that:
	/// - they are structs rather than classes,
	/// - they are mutable rather than readonly, and
	/// - their members (such as Item1, Item2, etc) are fields rather than properties.
	/// </summary>
	public struct ValueTuple : IValueTupleInternal, ITuple
	{
		/// <summary>
		/// Returns a value that indicates whether the current <see cref="ValueTuple"/> instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns><see langword="true"/> if <paramref name="obj"/> is a <see cref="ValueTuple"/>.</returns>

		int ITuple.Length => 0;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
#nullable enable
		object? ITuple.this[int index] => null;
#nullable disable
		/// <summary>Creates a new struct 0-tuple.</summary>
		/// <returns>A 0-tuple.</returns>
		public static ValueTuple Create()
		{
			return default;
		}

		/// <summary>Creates a new struct 1-tuple, or singleton.</summary>
		/// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
		/// <param name="item1">The value of the first component of the tuple.</param>
		/// <returns>A 1-tuple (singleton) whose value is (item1).</returns>
		public static ValueTuple<T1> Create<T1>(T1 item1)
		{
			return new(item1);
		}

		/// <summary>Creates a new struct 2-tuple, or pair.</summary>
		/// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
		/// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
		/// <param name="item1">The value of the first component of the tuple.</param>
		/// <param name="item2">The value of the second component of the tuple.</param>
		/// <returns>A 2-tuple (pair) whose value is (item1, item2).</returns>
		public static ValueTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
		{
			return new(item1, item2);
		}

		/// <summary>Creates a new struct 3-tuple, or triple.</summary>
		/// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
		/// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
		/// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
		/// <param name="item1">The value of the first component of the tuple.</param>
		/// <param name="item2">The value of the second component of the tuple.</param>
		/// <param name="item3">The value of the third component of the tuple.</param>
		/// <returns>A 3-tuple (triple) whose value is (item1, item2, item3).</returns>
		public static ValueTuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
		{
			return new(item1, item2, item3);
		}

		/// <summary>Creates a new struct 4-tuple, or quadruple.</summary>
		/// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
		/// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
		/// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
		/// <typeparam name="T4">The type of the fourth component of the tuple.</typeparam>
		/// <param name="item1">The value of the first component of the tuple.</param>
		/// <param name="item2">The value of the second component of the tuple.</param>
		/// <param name="item3">The value of the third component of the tuple.</param>
		/// <param name="item4">The value of the fourth component of the tuple.</param>
		/// <returns>A 4-tuple (quadruple) whose value is (item1, item2, item3, item4).</returns>
		public static ValueTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			return new(item1, item2, item3, item4);
		}

		/// <summary>Creates a new struct 5-tuple, or quintuple.</summary>
		/// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
		/// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
		/// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
		/// <typeparam name="T4">The type of the fourth component of the tuple.</typeparam>
		/// <typeparam name="T5">The type of the fifth component of the tuple.</typeparam>
		/// <param name="item1">The value of the first component of the tuple.</param>
		/// <param name="item2">The value of the second component of the tuple.</param>
		/// <param name="item3">The value of the third component of the tuple.</param>
		/// <param name="item4">The value of the fourth component of the tuple.</param>
		/// <param name="item5">The value of the fifth component of the tuple.</param>
		/// <returns>A 5-tuple (quintuple) whose value is (item1, item2, item3, item4, item5).</returns>
		public static ValueTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			return new(item1, item2, item3, item4, item5);
		}

		/// <summary>Creates a new struct 6-tuple, or sextuple.</summary>
		/// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
		/// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
		/// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
		/// <typeparam name="T4">The type of the fourth component of the tuple.</typeparam>
		/// <typeparam name="T5">The type of the fifth component of the tuple.</typeparam>
		/// <typeparam name="T6">The type of the sixth component of the tuple.</typeparam>
		/// <param name="item1">The value of the first component of the tuple.</param>
		/// <param name="item2">The value of the second component of the tuple.</param>
		/// <param name="item3">The value of the third component of the tuple.</param>
		/// <param name="item4">The value of the fourth component of the tuple.</param>
		/// <param name="item5">The value of the fifth component of the tuple.</param>
		/// <param name="item6">The value of the sixth component of the tuple.</param>
		/// <returns>A 6-tuple (sextuple) whose value is (item1, item2, item3, item4, item5, item6).</returns>
		public static ValueTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			return new ValueTuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
		}

		/// <summary>Creates a new struct 7-tuple, or septuple.</summary>
		/// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
		/// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
		/// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
		/// <typeparam name="T4">The type of the fourth component of the tuple.</typeparam>
		/// <typeparam name="T5">The type of the fifth component of the tuple.</typeparam>
		/// <typeparam name="T6">The type of the sixth component of the tuple.</typeparam>
		/// <typeparam name="T7">The type of the seventh component of the tuple.</typeparam>
		/// <param name="item1">The value of the first component of the tuple.</param>
		/// <param name="item2">The value of the second component of the tuple.</param>
		/// <param name="item3">The value of the third component of the tuple.</param>
		/// <param name="item4">The value of the fourth component of the tuple.</param>
		/// <param name="item5">The value of the fifth component of the tuple.</param>
		/// <param name="item6">The value of the sixth component of the tuple.</param>
		/// <param name="item7">The value of the seventh component of the tuple.</param>
		/// <returns>A 7-tuple (septuple) whose value is (item1, item2, item3, item4, item5, item6, item7).</returns>
		public static ValueTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			return new(item1, item2, item3, item4, item5, item6, item7);
		}

		/// <summary>Creates a new struct 8-tuple, or octuple.</summary>
		/// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
		/// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
		/// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
		/// <typeparam name="T4">The type of the fourth component of the tuple.</typeparam>
		/// <typeparam name="T5">The type of the fifth component of the tuple.</typeparam>
		/// <typeparam name="T6">The type of the sixth component of the tuple.</typeparam>
		/// <typeparam name="T7">The type of the seventh component of the tuple.</typeparam>
		/// <typeparam name="T8">The type of the eighth component of the tuple.</typeparam>
		/// <param name="item1">The value of the first component of the tuple.</param>
		/// <param name="item2">The value of the second component of the tuple.</param>
		/// <param name="item3">The value of the third component of the tuple.</param>
		/// <param name="item4">The value of the fourth component of the tuple.</param>
		/// <param name="item5">The value of the fifth component of the tuple.</param>
		/// <param name="item6">The value of the sixth component of the tuple.</param>
		/// <param name="item7">The value of the seventh component of the tuple.</param>
		/// <param name="item8">The value of the eighth component of the tuple.</param>
		/// <returns>An 8-tuple (octuple) whose value is (item1, item2, item3, item4, item5, item6, item7, item8).</returns>
		public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
		{
			return new(item1, item2, item3, item4, item5, item6, item7, Create(item8));
		}
	}

	/// <summary>Represents a 1-tuple, or singleton, as a value type.</summary>
	/// <typeparam name="T1">The type of the tuple's only component.</typeparam>
	public struct ValueTuple<T1> : IValueTupleInternal, ITuple
	{
		/// <summary>
		/// The current <see cref="ValueTuple{T1}"/> instance's first component.
		/// </summary>
		public T1 Item1;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueTuple{T1}"/> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		public ValueTuple(T1 item1)
		{
			Item1 = item1;
		}
		int ITuple.Length => 1;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
#nullable enable
		object? ITuple.this[int index] => index != 0 ? null : Item1;
#nullable disable
	}

	/// <summary>
	/// Represents a 2-tuple, or pair, as a value type.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1, T2> : IValueTupleInternal, ITuple
	{
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2}"/> instance's first component.
		/// </summary>
		public T1 Item1;

		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2}"/> instance's second component.
		/// </summary>
		public T2 Item2;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueTuple{T1, T2}"/> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		public ValueTuple(T1 item1, T2 item2)
		{
			Item1 = item1;
			Item2 = item2;
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 2;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
#nullable enable
		object? ITuple.this[int index] =>
#nullable disable

		index switch
		{
			0 => Item1,
			1 => Item2,
			_ => null,
		};
	}

	/// <summary>
	/// Represents a 3-tuple, or triple, as a value type.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
	public struct ValueTuple<T1, T2, T3> : IValueTupleInternal, ITuple
	{
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3}"/> instance's first component.
		/// </summary>
		public T1 Item1;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3}"/> instance's second component.
		/// </summary>
		public T2 Item2;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3}"/> instance's third component.
		/// </summary>
		public T3 Item3;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueTuple{T1, T2, T3}"/> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		/// <param name="item3">The value of the tuple's third component.</param>
		public ValueTuple(T1 item1, T2 item2, T3 item3)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 3;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
#nullable enable
		object? ITuple.this[int index] =>
#nullable disable

		index switch
		{
			0 => Item1,
			1 => Item2,
			2 => Item3,
			_ => null,
		};
	}

	/// <summary>
	/// Represents a 4-tuple, or quadruple, as a value type.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
	/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
	public struct ValueTuple<T1, T2, T3, T4> : IValueTupleInternal, ITuple
	{
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4}"/> instance's first component.
		/// </summary>
		public T1 Item1;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4}"/> instance's second component.
		/// </summary>
		public T2 Item2;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4}"/> instance's third component.
		/// </summary>
		public T3 Item3;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4}"/> instance's fourth component.
		/// </summary>
		public T4 Item4;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueTuple{T1, T2, T3, T4}"/> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		/// <param name="item3">The value of the tuple's third component.</param>
		/// <param name="item4">The value of the tuple's fourth component.</param>
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 4;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
#nullable enable
		object? ITuple.this[int index] =>
#nullable disable

		index switch
		{
			0 => Item1,
			1 => Item2,
			2 => Item3,
			3 => Item4,
			_ => null,
		};
	}

	/// <summary>
	/// Represents a 5-tuple, or quintuple, as a value type.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
	/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
	/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
	public struct ValueTuple<T1, T2, T3, T4, T5> : IValueTupleInternal, ITuple
	{
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5}"/> instance's first component.
		/// </summary>
		public T1 Item1;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5}"/> instance's second component.
		/// </summary>
		public T2 Item2;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5}"/> instance's third component.
		/// </summary>
		public T3 Item3;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5}"/> instance's fourth component.
		/// </summary>
		public T4 Item4;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5}"/> instance's fifth component.
		/// </summary>
		public T5 Item5;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueTuple{T1, T2, T3, T4, T5}"/> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		/// <param name="item3">The value of the tuple's third component.</param>
		/// <param name="item4">The value of the tuple's fourth component.</param>
		/// <param name="item5">The value of the tuple's fifth component.</param>
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 5;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
#nullable enable
		object? ITuple.this[int index] =>
#nullable disable

		index switch
		{
			0 => Item1,
			1 => Item2,
			2 => Item3,
			3 => Item4,
			4 => Item5,
			_ => null,
		};
	}

	/// <summary>
	/// Represents a 6-tuple, or sixtuple, as a value type.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
	/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
	/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
	/// <typeparam name="T6">The type of the tuple's sixth component.</typeparam>
	public struct ValueTuple<T1, T2, T3, T4, T5, T6> : IValueTupleInternal, ITuple
	{
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6}"/> instance's first component.
		/// </summary>
		public T1 Item1;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6}"/> instance's second component.
		/// </summary>
		public T2 Item2;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6}"/> instance's third component.
		/// </summary>
		public T3 Item3;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6}"/> instance's fourth component.
		/// </summary>
		public T4 Item4;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6}"/> instance's fifth component.
		/// </summary>
		public T5 Item5;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6}"/> instance's sixth component.
		/// </summary>
		public T6 Item6;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueTuple{T1, T2, T3, T4, T5, T6}"/> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		/// <param name="item3">The value of the tuple's third component.</param>
		/// <param name="item4">The value of the tuple's fourth component.</param>
		/// <param name="item5">The value of the tuple's fifth component.</param>
		/// <param name="item6">The value of the tuple's sixth component.</param>
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
			Item6 = item6;
		}



		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 6;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>

#nullable enable
		object? ITuple.this[int index] =>
#nullable disable

		index switch
		{
			0 => Item1,
			1 => Item2,
			2 => Item3,
			3 => Item4,
			4 => Item5,
			5 => Item6,
			_ => null,
		};
	}

	/// <summary>
	/// Represents a 7-tuple, or sentuple, as a value type.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
	/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
	/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
	/// <typeparam name="T6">The type of the tuple's sixth component.</typeparam>
	/// <typeparam name="T7">The type of the tuple's seventh component.</typeparam>
	/// 
	public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7> : IValueTupleInternal, ITuple
	{
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7}"/> instance's first component.
		/// </summary>
		public T1 Item1;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7}"/> instance's second component.
		/// </summary>
		public T2 Item2;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7}"/> instance's third component.
		/// </summary>
		public T3 Item3;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7}"/> instance's fourth component.
		/// </summary>
		public T4 Item4;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7}"/> instance's fifth component.
		/// </summary>
		public T5 Item5;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7}"/> instance's sixth component.
		/// </summary>
		public T6 Item6;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7}"/> instance's seventh component.
		/// </summary>
		public T7 Item7;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7}"/> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		/// <param name="item3">The value of the tuple's third component.</param>
		/// <param name="item4">The value of the tuple's fourth component.</param>
		/// <param name="item5">The value of the tuple's fifth component.</param>
		/// <param name="item6">The value of the tuple's sixth component.</param>
		/// <param name="item7">The value of the tuple's seventh component.</param>
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
			Item6 = item6;
			Item7 = item7;
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 7;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>

#nullable enable
		object? ITuple.this[int index] =>
#nullable disable

		index switch
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

	/// <summary>
	/// Represents an 8-tuple, or octuple, as a value type.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
	/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
	/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
	/// <typeparam name="T6">The type of the tuple's sixth component.</typeparam>
	/// <typeparam name="T7">The type of the tuple's seventh component.</typeparam>
	/// <typeparam name="TRest">The type of the tuple's eighth component.</typeparam>
	public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> : IValueTupleInternal, ITuple
	where TRest : struct
	{
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> instance's first component.
		/// </summary>
		public T1 Item1;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> instance's second component.
		/// </summary>
		public T2 Item2;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> instance's third component.
		/// </summary>
		public T3 Item3;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> instance's fourth component.
		/// </summary>
		public T4 Item4;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> instance's fifth component.
		/// </summary>
		public T5 Item5;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> instance's sixth component.
		/// </summary>
		public T6 Item6;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> instance's seventh component.
		/// </summary>
		public T7 Item7;
		/// <summary>
		/// The current <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> instance's eighth component.
		/// </summary>
		public TRest Rest;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		/// <param name="item3">The value of the tuple's third component.</param>
		/// <param name="item4">The value of the tuple's fourth component.</param>
		/// <param name="item5">The value of the tuple's fifth component.</param>
		/// <param name="item6">The value of the tuple's sixth component.</param>
		/// <param name="item7">The value of the tuple's seventh component.</param>
		/// <param name="rest">The value of the tuple's eight component.</param>
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
		{
			if (rest is not IValueTupleInternal)
			{
				rest = (TRest)(ITupleInternal)rest;
			}

			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
			Item6 = item6;
			Item7 = item7;
			Rest = rest;
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => Rest is IValueTupleInternal @internal ? 7 + @internal.Length : 8;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
#nullable enable
		object? ITuple.this[int index]
#nullable disable
		{
			get
			{
				switch (index)
				{
					case 0:
						return Item1;
					case 1:
						return Item2;
					case 2:
						return Item3;
					case 3:
						return Item4;
					case 4:
						return Item5;
					case 5:
						return Item6;
					case 6:
						return Item7;
					default:
						break;
				}

				return Rest is IValueTupleInternal @internal ? @internal[index - 7] : index == 7 ? Rest : null;
			}
		}
	}
}