namespace System
{
	public struct Int16
	{
		public const short MaxValue = 0x7FFF;
		public const short MinValue = unchecked((short)0x8000);
		public override string ToString()
		{
			return ((long)this).ToString();
		}
	}
}