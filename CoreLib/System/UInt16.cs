namespace System
{
	public struct UInt16
	{
		public override unsafe string ToString()
		{
			return ((ulong)this).ToString();
		}

		public string ToString(string format)
		{
			return ((ulong)this).ToString(format);
		}
	}
}