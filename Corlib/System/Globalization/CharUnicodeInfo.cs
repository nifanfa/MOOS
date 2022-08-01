namespace System.Globalization
{
	public static class CharUnicodeInfo
	{
		internal static bool IsWhiteSpace(string s, int index)
		{
			return InternalIsWhiteSpace(s[index]);
		}

		internal static bool IsWhiteSpace(char c)
		{
			return InternalIsWhiteSpace(c);
		}
		internal static bool InternalIsWhiteSpace(char c)
		{
			return c switch
			{
				'\u0020' or '\u00A0' or '\u1680' or '\u2000' or '\u2001' or '\u2002' or '\u2003' or '\u2004' or '\u2005' or '\u2006' or '\u2007' or '\u2008' or '\u2009' or '\u200A' or '\u202F' or '\u205F' or '\u3000' or '\u2028' or '\u2029' or '\u0009' or '\u000A' or '\u000B' or '\u000C' or '\u000D' or '\u0085' => true,
				_ => false,
			};
		}
	}
}