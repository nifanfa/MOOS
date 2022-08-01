namespace System
{
	public unsafe struct Boolean
	{
		internal const int True = 1;
		internal const int False = 0;
		internal const string TrueLiteral = "True";
		internal const string FalseLiteral = "False";
		public static readonly string TrueString = TrueLiteral;
		public static readonly string FalseString = FalseLiteral;
		public override string ToString()
		{
			return false == this ? FalseLiteral : TrueLiteral;
		}

		public static implicit operator bool(byte value)
		{
			return value != 0;
		}

		public static implicit operator bool(sbyte value)
		{
			return value != 0;
		}

		public static implicit operator bool(short value)
		{
			return value != 0;
		}

		public static implicit operator bool(ushort value)
		{
			return value != 0;
		}

		public static implicit operator bool(int value)
		{
			return value != 0;
		}

		public static implicit operator bool(uint value)
		{
			return value != 0;
		}

		public static implicit operator bool(long value)
		{
			return value != 0;
		}

		public static implicit operator bool(ulong value)
		{
			return value != 0;
		}

		public static implicit operator bool(float value)
		{
			return value != 0;
		}

		public static implicit operator bool(double value)
		{
			return value != 0;
		}

		public static implicit operator bool(void* value)
		{
			return value != null;
		}
	}
}