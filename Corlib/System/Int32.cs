namespace System
{
	public struct Int32
	{
		public const int MaxValue = 0x7fffffff;
		public const int MinValue = unchecked((int)0x80000000);

		public override string ToString()
		{
			return ((long)this).ToString();
		}

		public static implicit operator uint(int value)=>(uint)value;
	}
}