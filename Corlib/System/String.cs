using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Internal.Runtime.CompilerHelpers;
using Internal.Runtime.CompilerServices;

namespace System
{
	public sealed unsafe class String
	{
		[Intrinsic]
		public static readonly string Empty = "";


		// The layout of the string type is a contract with the compiler.
		private int _length;
		internal char _firstChar;


		public int Length
		{
			[Intrinsic]
			get => _length;
			set => _length = value;
		}

		public unsafe char this[int index]
		{
			[Intrinsic]
			get => Unsafe.Add(ref _firstChar, index);

			set
			{
				fixed (char* p = &_firstChar)
				{
					p[index] = value;
				}
			}
		}


#pragma warning disable CS0824 // Constructor is marked external
		public extern unsafe String(char* ptr);
		public extern String(IntPtr ptr);
		public extern String(char[] buf);
		public extern unsafe String(char* ptr, int index, int length);
		public extern unsafe String(char[] buf, int index, int length);
#pragma warning restore CS0824 // Constructor is marked external


		public static unsafe string FromASCII(nint ptr, int length)
		{
			byte* p = (byte*)ptr;
			char* newp = stackalloc char[length];
			for(int i = 0; i < length; i++)
            {
				newp[i] = (char)p[i];
            }
			return new string(newp, 0, length);
		}

		private static unsafe string Ctor(char* ptr)
		{
			int i = 0;

			while (ptr[i++] != '\0')
			{ }

			return Ctor(ptr, 0, i - 1);
		}
		private static unsafe string Ctor(IntPtr ptr)
		{
			return Ctor((char*)ptr);
		}
		private static unsafe string Ctor(char[] buf)
		{
			fixed (char* _buf = buf)
			{
				return Ctor(_buf, 0, buf.Length);
			}
		}
		private static unsafe string Ctor(char* ptr, int index, int length)
		{
			EETypePtr et = EETypePtr.EETypePtrOf<string>();

			char* start = ptr + index;
			object data = StartupCodeHelpers.RhpNewArray(et.Value, length);
			string s = Unsafe.As<object, string>(ref data);

			fixed (char* c = &s._firstChar)
			{
				memcpy((byte*)c, (byte*)start, (ulong)length * sizeof(char));
				c[length] = '\0';
			}

			return s;
		}
		[DllImport("*")]
		private static extern unsafe void memcpy(byte* dest, byte* src, ulong count);

		public int LastIndexOf(char j)
		{
			for (int i = Length - 1; i >= 0; i--)
			{
				if (this[i] == j)
				{
					return i;
				}
			}

			return -1;
		}

		private static unsafe string Ctor(char[] ptr, int index, int length)
		{
			fixed (char* _ptr = ptr)
			{
				return Ctor(_ptr, index, length);
			}
		}

		public override string ToString()
		{
			return this;
		}

		public override bool Equals(object obj)
		{
#pragma warning disable IDE0038 // Use pattern matching
			return obj is string && Equals((string)obj);
#pragma warning restore IDE0038 // Use pattern matching
		}

		public bool Equals(string val)
		{
			if (Length != val.Length)
			{
				return false;
			}

			for (int i = 0; i < Length; i++)
			{
				if (this[i] != val[i])
				{
					return false;
				}
			}

			return true;
		}

		public static bool operator ==(string a, string b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(string a, string b)
		{
			return !a.Equals(b);
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static string Concat(string a, string b)
		{
			int Length = a.Length + b.Length;
			char* ptr = stackalloc char[Length];
			int currentIndex = 0;
			for (int i = 0; i < a.Length; i++)
			{
				ptr[currentIndex] = a[i];
				currentIndex++;
			}
			for (int i = 0; i < b.Length; i++)
			{
				ptr[currentIndex] = b[i];
				currentIndex++;
			}
			return new string(ptr, 0, Length);
		}

		public int IndexOf(char j)
		{
			for (int i = 0; i < Length; i++)
			{
				if (this[i] == j)
				{
					return i;
				}
			}

			return -1;
		}

		public static string Concat(string a, string b, string c)
		{
			string p1 = a + b;
			string p2 = p1 + c;
			p1.Dispose();
			return p2;
		}

		public static string Concat(string a, string b, string c, string d)
		{
			string p1 = a + b;
			string p2 = p1 + c;
			string p3 = p2 + d;
			p1.Dispose();
			p2.Dispose();
			return p3;
		}

		public static string Concat(params string[] vs)
		{
			string s = "";
			for (int i = 0; i < vs.Length; i++)
			{
				string tmp = s + vs[i];
				s.Dispose();
				s = tmp;
			}
			vs.Dispose();
			return s;
		}
		public static string Format(string format, params object[] args)
		{
			lock (format)
			{
				string res = Empty;
				for (int i = 0; i < format.Length; i++)
				{
					string chr;
					if ((i + 2) < format.Length && format[i] == '{' && format[i + 2] == '}')
					{
						chr = args[format[i + 1] - 0x30].ToString();
						i += 2;
					} else
					{
						chr = format[i].ToString();
					}
					string str = res + chr;
					chr.Dispose();
					res.Dispose();
					res = str;
				}

				for (int i = 0; i < args.Length; i++)
				{
					args[i].Dispose();
				}

				args.Dispose();
				return res;
			}
		}

		public string Remove(int startIndex)
		{
			return Substring(0, startIndex);
		}

		public string[] Split(char chr)
		{
			List<string> strings = new();
			string tmp = string.Empty;
			for (int i = 0; i < Length; i++)
			{
				if (this[i] == chr)
				{
					strings.Add(tmp);
					tmp = string.Empty;
				} else
				{
					tmp += this[i];
				}

				if (i == (Length - 1))
				{
					strings.Add(tmp);
					tmp = string.Empty;
				}
			}
			return strings.ToArray();
		}

		public unsafe string Substring(int startIndex)
		{
			if ((Length == 0) && (startIndex == 0))
			{
				return Empty;
			}
			// Usually one uses the extension method with non-null values
			// so all we need to worry about is startIndex compared to value.Length.
			/*
			if ((startIndex < 0) || (startIndex >= Length))
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("startIndex");
			}
			*/

			/*
			string substring = "";
			for (int i = startIndex; i < Length; i++)
			{
				substring += this[i];
			}
			return substring;
			*/
			fixed (char* ptr = this)
			{
				return new string(ptr, startIndex, Length - startIndex);
			}
		}
		public unsafe string Substring(int startIndex, int endIndex)
		{
			if ((Length == 0) && (startIndex == 0))
			{
				return Empty;
			}
			// Usually one uses the extension method with non-null values
			// so all we need to worry about is startIndex compared to value.Length.
			/*
			if ((startIndex < 0) || (startIndex >= Length) || (endIndex >= Length) || (endIndex <= startIndex))
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("startIndex");
			}
			*/

			/*
			string substring = "";
			for (int i = startIndex; i < endIndex; i++)
			{
				substring += this[i];
			}
			return substring;
			*/
			fixed(char* ptr = this)
            {
				return new string(ptr, startIndex, endIndex - startIndex);
            }
		}
#nullable enable
		public static bool IsNullOrEmpty(string? value)
#nullable disable
		{
			return value == null || value.Length == 0;
		}
#nullable enable
		public static bool IsNullOrWhiteSpace(string? value)
#nullable disable
		{
			if (value == null)
			{
				value.Dispose();
				return true;
			}

			for (int i = 0; i < value.Length; i++)
			{
				if (!char.IsWhiteSpace(value[i]))
				{
					value.Dispose();
					return false;
				}
			}
			value.Dispose();
			return true;
		}

		public bool EndsWith(char value)
		{
			int thisLen = Length;
			if (thisLen != 0)
			{
				if (this[thisLen - 1] == value)
				{
					thisLen.Dispose();
					return true;
				}
			}
			thisLen.Dispose();
			return false;
		}

		public bool EndsWith(string value)
		{
			if (value.Length > Length)
			{
				return false;
			}

			if (value == this)
			{
				return true;
			}

			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] != this[Length - value.Length + i])
				{
					return false;
				}
			}
			return true;
		}

		public string ToUpper()
		{
			fixed (char* pthis = this)
			{
				string output = new string(pthis, 0, this.Length);
				for (int i = 0; i < this.Length; i++)
				{
					output[i] = pthis[i].ToUpper();
				}
				return output;
			}
		}
		public string ToLower()
		{
			fixed(char* pthis = this)
            {
				string output = new string(pthis, 0, this.Length);
				for(int i = 0; i < this.Length; i++)
                {
					output[i] = pthis[i].ToLower();
                }
				return output;
			}
		}
	}
}
/*
 * TODO: .NET String
####################
namespace System
{
	using System.Runtime.CompilerServices;
	using Internal.Runtime.CompilerServices;

	//
	// For Information on these methods, please see COMString.cpp
	//
	// The String class represents a static string of characters.  Many of
	// the String methods perform some type of transformation on the current
	// instance and return the result as a new String. All comparison methods are
	// implemented as a part of String.  As with arrays, character positions
	// (indices) are zero-based.
	//
	// When passing a null string into a constructor in VJ and VC, the null should be
	// explicitly type cast to a String.
	// For Example:
	// String s = new String((String)null);
	// Console.WriteLine(s);
	//
	public sealed class String
#if GENERICS_WORK
		, IComparable<String>, IEnumerable<char>, IEquatable<String>
#endif
	{

		private int m_stringLength;

		private char m_firstChar;

		private const int TrimHead = 0;
		private const int TrimTail = 1;
		private const int TrimBoth = 2;

		public static readonly string Empty;

		//
		//Native Static Methods
		//

		// Joins an array of strings together as one string with a separator between each original string.
		//
		public static string Join(string separator, params string[] value)
		{
			if (value == null)
			{
				ThrowHelpers.ThrowArgumentNullException("value");
				MOOS.Misc.Panic.Error("Argument Null Exception: value [String.Join]");
			}

			return Join(separator, value, 0, value.Length);
		}

		public static string Join(string separator, params object[] values)
		{
			if (values == null)
			{
				MOOS.Misc.Panic.Error("Argument Null Exception: values [String.Join]");
			}


			if (values.Length == 0 || values[0] == null)
			{
				return String.Empty;
			}

			separator ??= String.Empty;

			var result = "";
			string value = values[0].ToString();
			if (value != null)
			{
				result += value;
			}

			for (int i = 1; i < values.Length; i++)
			{
				result += separator;
				if (values[i] != null)
				{
					// handle the case where their ToString() override is broken
					value = values[i].ToString();
					if (value != null)
					{
						result += value;
					}
				}
			}
			return result;
		}


		private const int charPtrAlignConst = 1;
		private const int alignConst = 3;

		internal char FirstChar => m_firstChar;

		// Joins an array of strings together as one string with a separator between each original string.
		//
		public static unsafe string Join(string separator, string[] value, int startIndex, int count)
		{
			//Range check the array
			if (value == null)
			{
				MOOS.Misc.Panic.Error("Argument Null Exception: value [String.Join]");
			}

			if (startIndex < 0)
			{
				MOOS.Misc.Panic.Error("Argument Out Of Range Exception: startIndex [String.Join]");
				ThrowHelpers.ThrowArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}

			if (count < 0)
			{
				MOOS.Misc.Panic.Error("Argument Out Of Range Exception: count [String.Join]");
				ThrowHelpers.ThrowArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NegativeCount"));
			}

			if (startIndex > value.Length - count)
			{
				MOOS.Misc.Panic.Error("Argument Out Of Range Exception: startIndex [String.Join]");
				ThrowHelpers.ThrowArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_IndexCountBuffer"));
			}

			//Treat null as empty string.
			separator ??= String.Empty;

			//If count is 0, that skews a whole bunch of the calculations below, so just special case that.
			if (count == 0)
			{
				return String.Empty;
			}

			int jointLength = 0;
			//Figure out the total length of the strings in value
			int endIndex = startIndex + count - 1;
			for (int stringToJoinIndex = startIndex; stringToJoinIndex <= endIndex; stringToJoinIndex++)
			{
				if (value[stringToJoinIndex] != null)
				{
					jointLength += value[stringToJoinIndex].Length;
				}
			}

			//Add enough room for the separator.
			jointLength += (count - 1) * separator.Length;

			// Note that we may not catch all overflows with this check (since we could have wrapped around the 4gb range any number of times
			// and landed back in the positive range.) The input array might be modifed from other threads, 
			// so we have to do an overflow check before each append below anyway. Those overflows will get caught down there.
			if ((jointLength < 0) || ((jointLength + 1) < 0))
			{
				MOOS.Misc.Panic.Error("Out of memory Exception:  [String.Join]");
				ThrowHelpers.ThrowOutOfMemoryException();
			}

			//If this is an empty string, just return.
			if (jointLength == 0)
			{
				return String.Empty;
			}

			string jointString = FastAllocateString(jointLength);
			fixed (char* pointerToJointString = &jointString.m_firstChar)
			{
				string charBuffer = "";
				// Append the first string first and then append each following string prefixed by the separator.
				charBuffer += (value[startIndex]);
				for (int stringToJoinIndex = startIndex + 1; stringToJoinIndex <= endIndex; stringToJoinIndex++)
				{
					charBuffer += separator;
					charBuffer += value[stringToJoinIndex];
				}
			}

			return jointString;
		}

		private static unsafe int CompareOrdinalIgnoreCaseHelper(string strA, string strB)
		{
			int length = Math.Min(strA.Length, strB.Length);

			fixed (char* ap = &strA.m_firstChar)
			fixed (char* bp = &strB.m_firstChar)
			{
				char* a = ap;
				char* b = bp;

				while (length != 0)
				{
					int charA = *a;
					int charB = *b;

					// uppercase both chars - notice that we need just one compare per char
					if ((uint)(charA - 'a') <= 'z' - 'a')
					{
						charA -= 0x20;
					}

					if ((uint)(charB - 'a') <= 'z' - 'a')
					{
						charB -= 0x20;
					}

					//Return the (case-insensitive) difference between them.
					if (charA != charB)
					{
						return charA - charB;
					}

					// Next char
					a++;
					b++;
					length--;
				}

				return strA.Length - strB.Length;
			}
		}

		// This is a helper method for the security team.  They need to uppercase some strings (guaranteed to be less 
		// than 0x80) before security is fully initialized.  Without security initialized, we can't grab resources (the nlp's)
		// from the assembly.  This provides a workaround for that problem and should NOT be used anywhere else.
		//
		internal static unsafe string SmallCharToUpper(string strIn)
		{
			//
			// Get the length and pointers to each of the buffers.  Walk the length
			// of the string and copy the characters from the inBuffer to the outBuffer,
			// capitalizing it if necessary.  We assert that all of our characters are
			// less than 0x80.
			//
			int length = strIn.Length;
			string strOut = FastAllocateString(length);
			fixed (char* inBuff = &strIn.m_firstChar, outBuff = &strOut.m_firstChar)
			{

				for (int i = 0; i < length; i++)
				{
					int c = inBuff[i];

					// uppercase - notice that we need just one compare
					if ((uint)(c - 'a') <= 'z' - 'a')
					{
						c -= 0x20;
					}

					outBuff[i] = (char)c;
				}
			}
			return strOut;
		}

		//
		//
		// NATIVE INSTANCE METHODS
		//
		//

		//
		// Search/Query methods
		//

		private static unsafe bool EqualsHelper(string strA, string strB)
		{
			int length = strA.Length;

			fixed (char* ap = &strA.m_firstChar)
			fixed (char* bp = &strB.m_firstChar)
			{
				char* a = ap;
				char* b = bp;

				// unroll the loop
//#if AMD64
				// for AMD64 bit platform we unroll by 12 and
				// check 3 qword at a time. This is less code
				// than the 32 bit case and is shorter
				// pathlength

				while (length >= 12)
				{
					if (*(long*)a     != *(long*)b) return false;
					if (*(long*)(a+4) != *(long*)(b+4)) return false;
					if (*(long*)(a+8) != *(long*)(b+8)) return false;
					a += 12; b += 12; length -= 12;
				}
//#else
				*//*while (length >= 10)
				{
					if (*(int*)a != *(int*)b)
					{
						return false;
					}

					if (*(int*)(a + 2) != *(int*)(b + 2))
					{
						return false;
					}

					if (*(int*)(a + 4) != *(int*)(b + 4))
					{
						return false;
					}

					if (*(int*)(a + 6) != *(int*)(b + 6))
					{
						return false;
					}

					if (*(int*)(a + 8) != *(int*)(b + 8))
					{
						return false;
					}

					a += 10;
					b += 10;
					length -= 10;
				}
#endif*//*

				// This depends on the fact that the String objects are
				// always zero terminated and that the terminating zero is not included
				// in the length. For odd string sizes, the last compare will include
				// the zero terminator.
				while (length > 0)
				{
					if (*(int*)a != *(int*)b)
					{
						break;
					}

					a += 2;
					b += 2;
					length -= 2;
				}

				return length <= 0;
			}
		}

		private static unsafe bool EqualsIgnoreCaseAsciiHelper(string strA, string strB)
		{
			int length = strA.Length;

			fixed (char* ap = &strA.m_firstChar)
			fixed (char* bp = &strB.m_firstChar)
			{
				char* a = ap;
				char* b = bp;

				while (length != 0)
				{
					int charA = *a;
					int charB = *b;

					// Ordinal equals or lowercase equals if the result ends up in the a-z range 
					if (charA == charB ||
					   ((charA | 0x20) == (charB | 0x20) &&
						  (uint)((charA | 0x20) - 'a') <= 'z' - 'a'))
					{
						a++;
						b++;
						length--;
					} else
					{
						// We use goto for perf reasons. x86 JIT does not optimize around gotos.
						goto ReturnFalse;
					}
				}

				return true;

			ReturnFalse:
				return false;
			}
		}

		private static unsafe int CompareOrdinalHelper(string strA, string strB)
		{
			int length = Math.Min(strA.Length, strB.Length);
			int diffOffset = -1;

			fixed (char* ap = &strA.m_firstChar)
			fixed (char* bp = &strB.m_firstChar)
			{
				char* a = ap;
				char* b = bp;

				// unroll the loop
				while (length >= 10)
				{
					if (*(int*)a != *(int*)b)
					{
						diffOffset = 0;
						break;
					}

					if (*(int*)(a + 2) != *(int*)(b + 2))
					{
						diffOffset = 2;
						break;
					}

					if (*(int*)(a + 4) != *(int*)(b + 4))
					{
						diffOffset = 4;
						break;
					}

					if (*(int*)(a + 6) != *(int*)(b + 6))
					{
						diffOffset = 6;
						break;
					}

					if (*(int*)(a + 8) != *(int*)(b + 8))
					{
						diffOffset = 8;
						break;
					}
					a += 10;
					b += 10;
					length -= 10;
				}

				if (diffOffset != -1)
				{
					// we already see a difference in the unrolled loop above
					a += diffOffset;
					b += diffOffset;
					int order;
					if ((order = *a - *b) != 0)
					{
						return order;
					}
					return *(a + 1) - *(b + 1);
				}

				// now go back to slower code path and do comparison on 4 bytes one time.
				// Following code also take advantage of the fact strings will 
				// use even numbers of characters (runtime will have a extra zero at the end.)
				// so even if length is 1 here, we can still do the comparsion.  
				while (length > 0)
				{
					if (*(int*)a != *(int*)b)
					{
						break;
					}
					a += 2;
					b += 2;
					length -= 2;
				}

				if (length > 0)
				{
					int c;
					// found a different int on above loop
					if ((c = *a - *b) != 0)
					{
						return c;
					}
					return *(a + 1) - *(b + 1);
				}

				// At this point, we have compared all the characters in at least one string.
				// The longer string will be larger.
				return strA.Length - strB.Length;
			}
		}

		// Determines whether two strings match.
		public override bool Equals(object obj)
		{
			if (this == null)                        //this is necessary to guard against reverse-pinvokes and
			{
				MOOS.Misc.Panic.Error("Null Reference Exception:  [String.Equals]");
				ThrowHelpers.ThrowNullReferenceException();  //other callers who do not use the callvirt instruction
			}

			string str = obj as string;
			if (str == null)
			{
				return false;
			}

			if (this.Equals(obj))
			{
				return true;
			}

			return Length == str.Length && EqualsHelper(this, str);
		}

		// Determines whether two strings match.
		public bool Equals(string value)
		{
			if (this == null)                        //this is necessary to guard against reverse-pinvokes and
			{
				MOOS.Misc.Panic.Error("Null Reference Exception:  [String.Equals]");
				ThrowHelpers.ThrowNullReferenceException();  //other callers who do not use the callvirt instruction
			}

			if (value == null)
			{
				return false;
			}

			if (this.Equals(value))
			{
				return true;
			}

			return Length == value.Length && EqualsHelper(this, value);
		}

		// Determines whether two Strings match.
		public static bool Equals(string a, string b)
		{
			if (a == (object)b)
			{
				return true;
			}

			if (a is null || b is null)
			{
				return false;
			}

			return a.Length == b.Length && EqualsHelper(a, b);
		}

		public static bool operator ==(string a, string b)
		{
			return String.Equals(a, b);
		}

		public static bool operator !=(string a, string b)
		{
			return !String.Equals(a, b);
		}

		// Gets the character at a specified position.
		//
		// Spec#: Apply the precondition here using a contract assembly.  Potential perf issue.

		public unsafe char this[int index]
		{
			[Intrinsic]
			get => Unsafe.Add(ref m_firstChar, index);

			set
			{
				fixed (char* p = &m_firstChar)
				{
					p[index] = value;
				}
			}
		}

		// Converts a substring of this string to an array of characters.  Copies the
		// characters of this string beginning at position startIndex and ending at
		// startIndex + length - 1 to the character array buffer, beginning
		// at bufferStartIndex.
		//
		public unsafe void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
		{
			if (destination == null)
			{
				ThrowHelpers.ThrowArgumentNullException("destination");
			}

			if (count < 0)
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NegativeCount"));
			}

			if (sourceIndex < 0)
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("sourceIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}

			if (count > Length - sourceIndex)
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("sourceIndex", Environment.GetResourceString("ArgumentOutOfRange_IndexCount"));
			}

			if (destinationIndex > destination.Length - count || destinationIndex < 0)
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("destinationIndex", Environment.GetResourceString("ArgumentOutOfRange_IndexCount"));
			}


			// Note: fixed does not like empty arrays
			if (count > 0)
			{
				fixed (char* src = &m_firstChar)
				fixed (char* dest = destination)
				{
					wstrcpy(dest + destinationIndex, src + sourceIndex, count);
				}
			}
		}

		// Returns the entire string as an array of characters.
		public unsafe char[] ToCharArray()
		{
			// <
			int length = Length;
			char[] chars = new char[length];
			if (length > 0)
			{
				fixed (char* src = &m_firstChar)
				fixed (char* dest = chars)
				{
					wstrcpy(dest, src, length);
				}
			}
			return chars;
		}

		// Returns a substring of this string as an array of characters.
		//
		public unsafe char[] ToCharArray(int startIndex, int length)
		{
			// Range check everything.
			if (startIndex < 0 || startIndex > Length || startIndex > Length - length)
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}

			if (length < 0)
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}

			
			char[] chars = new char[length];
			if (length > 0)
			{
				fixed (char* src = &m_firstChar)
				fixed (char* dest = chars)
				{
					wstrcpy(dest, src + startIndex, length);
				}
			}
			return chars;
		}

		public static bool IsNullOrEmpty(string value)
		{
			return value == null || value.Length == 0;
		}

		public static bool IsNullOrWhiteSpace(string value)
		{
			if (value == null)
			{
				return true;
			}

			for (int i = 0; i < value.Length; i++)
			{
				if (!char.IsWhiteSpace(value[i]))
				{
					return false;
				}
			}

			return true;
		}

		// Gets a hash code for this string.  If strings A and B are such that A.Equals(B), then
		// they will return the same hash code.
		public override int GetHashCode()
		{

#if FEATURE_RANDOMIZED_STRING_HASHING
			if(HashHelpers.s_UseRandomizedStringHashing)
			{
				return InternalMarvin32HashString(this, this.Length, 0);
			}
#endif // FEATURE_RANDOMIZED_STRING_HASHING

			unsafe
			{
				fixed (char* src = this)
				{
					int hash1 = 5381;
					int hash2 = hash1;

					int c;
					char* s = src;
					while ((c = s[0]) != 0)
					{
						hash1 = ((hash1 << 5) + hash1) ^ c;
						c = s[1];
						if (c == 0)
						{
							break;
						}

						hash2 = ((hash2 << 5) + hash2) ^ c;
						s += 2;
					}
					return hash1 + (hash2 * 1566083941);
				}
			}
		}

		// Use this if and only if you need the hashcode to not change across app domains (e.g. you have an app domain agile
		// hash table).
		internal int GetLegacyNonRandomizedHashCode()
		{
			unsafe
			{
				fixed (char* src = this)
				{
					int hash1 = 5381;
					int hash2 = hash1;

					int c;
					char* s = src;
					while ((c = s[0]) != 0)
					{
						hash1 = ((hash1 << 5) + hash1) ^ c;
						c = s[1];
						if (c == 0)
						{
							break;
						}

						hash2 = ((hash2 << 5) + hash2) ^ c;
						s += 2;
					}
					return hash1 + (hash2 * 1566083941);
				}
			}
		}

		// Gets the length of this string
		//
		/// This is a EE implemented function so that the JIT can recognise is specially
		/// and eliminate checks on character fetchs in a loop like:
		///        for(int I = 0; I < str.Length; i++) str[i]
		/// The actually code generated for this will be one instruction and will be inlined.
		//
		// Spec#: Add postcondition in a contract assembly.  Potential perf problem.
		public int Length
		{
			get => m_stringLength;
		}

		// Creates an array of strings by splitting this string at each
		// occurence of a separator.  The separator is searched for, and if found,
		// the substring preceding the occurence is stored as the first element in
		// the array of strings.  We then continue in this manner by searching
		// the substring that follows the occurence.  On the other hand, if the separator
		// is not found, the array of strings will contain this instance as its only element.
		// If the separator is null
		// whitespace (i.e., Character.IsWhitespace) is used as the separator.
		//
		public string[] Split(params char[] separator)
		{
			return SplitInternal(separator, int.MaxValue, StringSplitOptions.None);
		}

		// Creates an array of strings by splitting this string at each
		// occurence of a separator.  The separator is searched for, and if found,
		// the substring preceding the occurence is stored as the first element in
		// the array of strings.  We then continue in this manner by searching
		// the substring that follows the occurence.  On the other hand, if the separator
		// is not found, the array of strings will contain this instance as its only element.
		// If the spearator is the empty string (i.e., String.Empty), then
		// whitespace (i.e., Character.IsWhitespace) is used as the separator.
		// If there are more than count different strings, the last n-(count-1)
		// elements are concatenated and added as the last String.
		//
		public string[] Split(char[] separator, int count)
		{
			return SplitInternal(separator, count, StringSplitOptions.None);
		}

		public string[] Split(char[] separator, StringSplitOptions options)
		{
			return SplitInternal(separator, int.MaxValue, options);
		}

		public string[] Split(char[] separator, int count, StringSplitOptions options)
		{
			return SplitInternal(separator, count, options);
		}

		internal string[] SplitInternal(char[] separator, int count, StringSplitOptions options)
		{
			if (count < 0)
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("count",
					//Environment.GetResourceString("ArgumentOutOfRange_NegativeCount"));
			}

			if (options < StringSplitOptions.None || options > StringSplitOptions.RemoveEmptyEntries)
			{
				ThrowHelpers.ThrowArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", options));
			}

			bool omitEmptyEntries = options == StringSplitOptions.RemoveEmptyEntries;

			if ((count == 0) || (omitEmptyEntries && Length == 0))
			{
				return new string[0];
			}

			int[] sepList = new int[Length];
			int numReplaces = MakeSeparatorList(separator, ref sepList);

			//Handle the special case of no replaces and special count.
			if (0 == numReplaces || count == 1)
			{
				string[] stringArray = new string[1];
				stringArray[0] = this;
				return stringArray;
			}

			return omitEmptyEntries
				? InternalSplitOmitEmptyEntries(sepList, null, numReplaces, count)
				: InternalSplitKeepEmptyEntries(sepList, null, numReplaces, count);
		}

		public string[] Split(string[] separator, StringSplitOptions options)
		{
			return Split(separator, int.MaxValue, options);
		}

		public string[] Split(string[] separator, int count, StringSplitOptions options)
		{
			if (count < 0)
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("count",
				  //  Environment.GetResourceString("ArgumentOutOfRange_NegativeCount"));
			}

			if (options < StringSplitOptions.None || options > StringSplitOptions.RemoveEmptyEntries)
			{
				ThrowHelpers.ThrowArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", (int)options));
			}
			//Contract.EndContractBlock();

			bool omitEmptyEntries = options == StringSplitOptions.RemoveEmptyEntries;

			if (separator == null || separator.Length == 0)
			{
				return SplitInternal(null, count, options);
			}

			if ((count == 0) || (omitEmptyEntries && Length == 0))
			{
				return new string[0];
			}

			int[] sepList = new int[Length];
			int[] lengthList = new int[Length];
			int numReplaces = MakeSeparatorList(separator, ref sepList, ref lengthList);

			//Handle the special case of no replaces and special count.
			if (0 == numReplaces || count == 1)
			{
				string[] stringArray = new string[1];
				stringArray[0] = this;
				return stringArray;
			}

			return omitEmptyEntries
				? InternalSplitOmitEmptyEntries(sepList, lengthList, numReplaces, count)
				: InternalSplitKeepEmptyEntries(sepList, lengthList, numReplaces, count);
		}

		// Note a few special case in this function:
		//     If there is no separator in the string, a string array which only contains 
		//     the original string will be returned regardless of the count. 
		//

		private string[] InternalSplitKeepEmptyEntries(int[] sepList, int[] lengthList, int numReplaces, int count)
		{
			int currIndex = 0;
			int arrIndex = 0;

			count--;
			int numActualReplaces = (numReplaces < count) ? numReplaces : count;

			//Allocate space for the new array.
			//+1 for the string from the end of the last replace to the end of the String.
			string[] splitStrings = new string[numActualReplaces + 1];

			for (int i = 0; i < numActualReplaces && currIndex < Length; i++)
			{
				splitStrings[arrIndex++] = Substring(currIndex, sepList[i] - currIndex);
				currIndex = sepList[i] + ((lengthList == null) ? 1 : lengthList[i]);
			}

			//Handle the last string at the end of the array if there is one.
			if (currIndex < Length && numActualReplaces >= 0)
			{
				splitStrings[arrIndex] = Substring(currIndex);
			} else if (arrIndex == numActualReplaces)
			{
				//We had a separator character at the end of a string.  Rather than just allowing
				//a null character, we'll replace the last element in the array with an empty string.
				splitStrings[arrIndex] = String.Empty;

			}

			return splitStrings;
		}


		// This function will not keep the Empty String 
		private string[] InternalSplitOmitEmptyEntries(int[] sepList, int[] lengthList, int numReplaces, int count)
		{
			// Allocate array to hold items. This array may not be 
			// filled completely in this function, we will create a 
			// new array and copy string references to that new array.

			int maxItems = (numReplaces < count) ? (numReplaces + 1) : count;
			string[] splitStrings = new string[maxItems];

			int currIndex = 0;
			int arrIndex = 0;

			for (int i = 0; i < numReplaces && currIndex < Length; i++)
			{
				if (sepList[i] - currIndex > 0)
				{
					splitStrings[arrIndex++] = Substring(currIndex, sepList[i] - currIndex);
				}
				currIndex = sepList[i] + ((lengthList == null) ? 1 : lengthList[i]);
				if (arrIndex == count - 1)
				{
					// If all the remaining entries at the end are empty, skip them
					while (i < numReplaces - 1 && currIndex == sepList[++i])
					{
						currIndex += (lengthList == null) ? 1 : lengthList[i];
					}
					break;
				}
			}


			//Handle the last string at the end of the array if there is one.
			if (currIndex < Length)
			{
				splitStrings[arrIndex++] = Substring(currIndex);
			}

			string[] stringArray = splitStrings;
			if (arrIndex != maxItems)
			{
				stringArray = new string[arrIndex];
				for (int j = 0; j < arrIndex; j++)
				{
					stringArray[j] = splitStrings[j];
				}
			}
			return stringArray;
		}

		//--------------------------------------------------------------------
		// This function returns number of the places within baseString where
		// instances of characters in Separator occur.
		// Args: separator  -- A string containing all of the split characters.
		//       sepList    -- an array of ints for split char indicies.
		//--------------------------------------------------------------------
		private unsafe int MakeSeparatorList(char[] separator, ref int[] sepList)
		{
			int foundCount = 0;

			if (separator == null || separator.Length == 0)
			{
				fixed (char* pwzChars = &m_firstChar)
				{
					//If they passed null or an empty string, look for whitespace.
					for (int i = 0; i < Length && foundCount < sepList.Length; i++)
					{
						if (char.IsWhiteSpace(pwzChars[i]))
						{
							sepList[foundCount++] = i;
						}
					}
				}
			} else
			{
				int sepListCount = sepList.Length;
				int sepCount = separator.Length;
				//If they passed in a string of chars, actually look for those chars.
				fixed (char* pwzChars = &m_firstChar, pSepChars = separator)
				{
					for (int i = 0; i < Length && foundCount < sepListCount; i++)
					{
						char* pSep = pSepChars;
						for (int j = 0; j < sepCount; j++, pSep++)
						{
							if (pwzChars[i] == *pSep)
							{
								sepList[foundCount++] = i;
								break;
							}
						}
					}
				}
			}
			return foundCount;
		}

		//--------------------------------------------------------------------
		// This function returns number of the places within baseString where
		// instances of separator strings occur.
		// Args: separators -- An array containing all of the split strings.
		//       sepList    -- an array of ints for split string indicies.
		//       lengthList -- an array of ints for split string lengths.
		//--------------------------------------------------------------------
		private unsafe int MakeSeparatorList(string[] separators, ref int[] sepList, ref int[] lengthList)
		{
			int foundCount = 0;
			int sepListCount = sepList.Length;
			int sepCount = separators.Length;

			fixed (char* pwzChars = &m_firstChar)
			{
				for (int i = 0; i < Length && foundCount < sepListCount; i++)
				{
					for (int j = 0; j < separators.Length; j++)
					{
						string separator = separators[j];
						if (String.IsNullOrEmpty(separator))
						{
							continue;
						}
						int currentSepLength = separator.Length;
						if (pwzChars[i] == separator[0] && currentSepLength <= Length - i)
						{
							if (currentSepLength == 1
								|| String.CompareOrdinal(this, i, separator, 0, currentSepLength) == 0)
							{
								sepList[foundCount] = i;
								lengthList[foundCount] = currentSepLength;
								foundCount++;
								i += currentSepLength - 1;
								break;
							}
						}
					}
				}
			}
			return foundCount;
		}

		// Returns a substring of this string.
		//
		public string Substring(int startIndex)
		{
			return Substring(startIndex, Length - startIndex);
		}

		// Returns a substring of this string.
		//
		public string Substring(int startIndex, int length)
		{

			//Bounds Checking.
			if (startIndex < 0)
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}

			if (startIndex > Length)
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndexLargerThanLength"));
			}

			if (length < 0)
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NegativeLength"));
			}

			if (startIndex > Length - length)
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_IndexLength"));
			}

			if (length == 0)
			{
				return String.Empty;
			}

			return startIndex == 0 && length == Length ? this : InternalSubString(startIndex, length);
		}

		private unsafe string InternalSubString(int startIndex, int length)
		{
			string result = FastAllocateString(length);

			fixed (char* dest = &result.m_firstChar)
			fixed (char* src = &m_firstChar)
			{
				wstrcpy(dest, src + startIndex, length);
			}

			return result;
		}


		// Removes a string of characters from the ends of this string.
		public string Trim(params char[] trimChars)
		{
			return null == trimChars || trimChars.Length == 0 ? TrimHelper(TrimBoth) : TrimHelper(trimChars, TrimBoth);
		}

		// Removes a string of characters from the beginning of this string.
		public string TrimStart(params char[] trimChars)
		{
			return null == trimChars || trimChars.Length == 0 ? TrimHelper(TrimHead) : TrimHelper(trimChars, TrimHead);
		}


		// Removes a string of characters from the end of this string.
		public string TrimEnd(params char[] trimChars)
		{
			return null == trimChars || trimChars.Length == 0 ? TrimHelper(TrimTail) : TrimHelper(trimChars, TrimTail);
		}


		// Creates a new string with the characters copied in from ptr. If
		// ptr is null, a 0-length string (like String.Empty) is returned.
		//
		
#pragma warning disable CS0824 // Constructor is marked external
		public extern unsafe String(char* ptr);
		public extern String(IntPtr ptr);
		public extern String(char[] buf);
		public extern unsafe String(char* ptr, int index, int length);
		public extern unsafe String(char[] buf, int index, int length);
#pragma warning restore CS0824 // Constructor is marked external


		internal static string FastAllocateString(int length)
		{
			return new(Allocator.Allocate((ulong)(2 * length)));
		}

		private static unsafe void FillStringChecked(string dest, int destPos, string src)
		{
			if (src.Length > dest.Length - destPos)
			{
				ThrowHelpers.ThrowIndexOutOfRangeException();
			}

			fixed (char* pDest = &dest.m_firstChar)
			fixed (char* pSrc = &src.m_firstChar)
			{
				wstrcpy(pDest + destPos, pSrc, src.Length);
			}
		}

		// Creates a new string from the characters in a subarray.  The new string will
		// be created from the characters in value between startIndex and
		// startIndex + length - 1.
		//
		[System.Security.SecuritySafeCritical]  // auto-generated
		[ResourceExposure(ResourceScope.None)]
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public extern String(char[] value, int startIndex, int length);

		// Creates a new string from the characters in a subarray.  The new string will be
		// created from the characters in value.
		//

		[System.Security.SecuritySafeCritical]  // auto-generated
		[ResourceExposure(ResourceScope.None)]
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public extern String(char[] value);

		[System.Security.SecurityCritical]  // auto-generated
		internal static unsafe void wstrcpy(char* dmem, char* smem, int charCount)
		{
			Buffer.Memcpy((byte*)dmem, (byte*)smem, charCount * 2); // 2 used everywhere instead of sizeof(char)
		}

		[System.Security.SecuritySafeCritical]  // auto-generated
		private string CtorCharArray(char[] value)
		{
			if (value != null && value.Length != 0)
			{
				string result = FastAllocateString(value.Length);

				unsafe
				{
					fixed (char* dest = result, source = value)
					{
						wstrcpy(dest, source, value.Length);
					}
				}
				return result;
			} else
			{
				return String.Empty;
			}
		}

		[System.Security.SecuritySafeCritical]  // auto-generated
		private string CtorCharArrayStartLength(char[] value, int startIndex, int length)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}

			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NegativeLength"));
			}

			if (startIndex > value.Length - length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}

			Contract.EndContractBlock();

			if (length > 0)
			{
				string result = FastAllocateString(length);

				unsafe
				{
					fixed (char* dest = result, source = value)
					{
						wstrcpy(dest, source + startIndex, length);
					}
				}
				return result;
			} else
			{
				return String.Empty;
			}
		}

		[System.Security.SecuritySafeCritical]  // auto-generated
		private string CtorCharCount(char c, int count)
		{
			if (count > 0)
			{
				string result = FastAllocateString(count);
				if (c != 0)
				{
					unsafe
					{
						fixed (char* dest = result)
						{
							char* dmem = dest;
							while (((uint)dmem & 3) != 0 && count > 0)
							{
								*dmem++ = c;
								count--;
							}
							uint cc = (uint)((c << 16) | c);
							if (count >= 4)
							{
								count -= 4;
								do
								{
									((uint*)dmem)[0] = cc;
									((uint*)dmem)[1] = cc;
									dmem += 4;
									count -= 4;
								} while (count >= 0);
							}
							if ((count & 2) != 0)
							{
								((uint*)dmem)[0] = cc;
								dmem += 2;
							}
							if ((count & 1) != 0)
							{
								dmem[0] = c;
							}
						}
					}
				}
				return result;
			} else
			{
				return count == 0
				? String.Empty
				: throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_MustBeNonNegNum", "count"));
			}
		}

		[System.Security.SecurityCritical]  // auto-generated
		private static unsafe int wcslen(char* ptr)
		{
			char* end = ptr;

			// The following code is (somewhat surprisingly!) significantly faster than a naive loop,
			// at least on x86 and the current jit.

			// First make sure our pointer is aligned on a dword boundary
			while (((uint)end & 3) != 0 && *end != 0)
			{
				end++;
			}

			if (*end != 0)
			{
				// The loop condition below works because if "end[0] & end[1]" is non-zero, that means
				// neither operand can have been zero. If is zero, we have to look at the operands individually,
				// but we hope this going to fairly rare.

				// In general, it would be incorrect to access end[1] if we haven't made sure
				// end[0] is non-zero. However, we know the ptr has been aligned by the loop above
				// so end[0] and end[1] must be in the same page, so they're either both accessible, or both not.

				while ((end[0] & end[1]) != 0 || (end[0] != 0 && end[1] != 0))
				{
					end += 2;
				}
			}
			// finish up with the naive loop
			for (; *end != 0; end++)
			{
				;
			}

			int count = (int)(end - ptr);

			return count;
		}

		[System.Security.SecurityCritical]  // auto-generated
		private unsafe string CtorCharPtr(char* ptr)
		{
			if (ptr == null)
			{
				return String.Empty;
			}

#if !FEATURE_PAL
			if (ptr < (char*)64000)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeStringPtrNotAtom"));
			}
#endif // FEATURE_PAL

			Contract.Assert(this == null, "this == null");        // this is the string constructor, we allocate it

			try
			{
				int count = wcslen(ptr);
				if (count == 0)
				{
					return String.Empty;
				}

				string result = FastAllocateString(count);
				fixed (char* dest = result)
				{
					wstrcpy(dest, ptr, count);
				}

				return result;
			} catch (NullReferenceException)
			{
				throw new ArgumentOutOfRangeException("ptr", Environment.GetResourceString("ArgumentOutOfRange_PartialWCHAR"));
			}
		}

		[System.Security.SecurityCritical]  // auto-generated
		private unsafe string CtorCharPtrStartLength(char* ptr, int startIndex, int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NegativeLength"));
			}

			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}
			Contract.EndContractBlock();
			Contract.Assert(this == null, "this == null");        // this is the string constructor, we allocate it

			char* pFrom = ptr + startIndex;
			if (pFrom < ptr)
			{
				// This means that the pointer operation has had an overflow
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_PartialWCHAR"));
			}

			if (length == 0)
			{
				return String.Empty;
			}

			string result = FastAllocateString(length);

			try
			{
				fixed (char* dest = result)
				{
					wstrcpy(dest, pFrom, length);
				}

				return result;
			} catch (NullReferenceException)
			{
				throw new ArgumentOutOfRangeException("ptr", Environment.GetResourceString("ArgumentOutOfRange_PartialWCHAR"));
			}
		}

		[System.Security.SecuritySafeCritical]  // auto-generated
		[ResourceExposure(ResourceScope.None)]
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public extern String(char c, int count);

		//
		//
		// INSTANCE METHODS
		//
		//

		// Provides a culture-correct string comparison. StrA is compared to StrB
		// to determine whether it is lexicographically less, equal, or greater, and then returns
		// either a negative integer, 0, or a positive integer; respectively.
		//
		[Pure]
		public static int Compare(string strA, string strB)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
		}


		// Provides a culture-correct string comparison. strA is compared to strB
		// to determine whether it is lexicographically less, equal, or greater, and then a
		// negative integer, 0, or a positive integer is returned; respectively.
		// The case-sensitive option is set by ignoreCase
		//
		[Pure]
		public static int Compare(string strA, string strB, bool ignoreCase)
		{
			return ignoreCase
				? CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase)
				: CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
		}


		// Provides a more flexible function for string comparision. See StringComparison 
		// for meaning of different comparisonType.
		[Pure]
		[System.Security.SecuritySafeCritical]  // auto-generated
		public static int Compare(string strA, string strB, StringComparison comparisonType)
		{
			// Single comparison to check if comparisonType is within [CurrentCulture .. OrdinalIgnoreCase]
			if ((uint)(comparisonType - StringComparison.CurrentCulture) >
				(uint)(StringComparison.OrdinalIgnoreCase - StringComparison.CurrentCulture))
			{
				throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
			Contract.EndContractBlock();

			if (strA == (object)strB)
			{
				return 0;
			}

			//they can't both be null;
			if (strA == null)
			{
				return -1;
			}

			if (strB == null)
			{
				return 1;
			}

			switch (comparisonType)
			{
				case StringComparison.CurrentCulture:
					return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);

				case StringComparison.CurrentCultureIgnoreCase:
					return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase);

				case StringComparison.InvariantCulture:
					return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);

				case StringComparison.InvariantCultureIgnoreCase:
					return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase);

				case StringComparison.Ordinal:
					// Most common case: first character is different.
					if ((strA.m_firstChar - strB.m_firstChar) != 0)
					{
						return strA.m_firstChar - strB.m_firstChar;
					}

					return CompareOrdinalHelper(strA, strB);

				case StringComparison.OrdinalIgnoreCase:
					// If both strings are ASCII strings, we can take the fast path.
					if (strA.IsAscii() && strB.IsAscii())
					{
						return CompareOrdinalIgnoreCaseHelper(strA, strB);
					}
					// Take the slow path.                
					return TextInfo.CompareOrdinalIgnoreCase(strA, strB);

				default:
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_StringComparison"));
			}
		}


		// Provides a culture-correct string comparison. strA is compared to strB
		// to determine whether it is lexicographically less, equal, or greater, and then a
		// negative integer, 0, or a positive integer is returned; respectively.
		//
		[Pure]
		public static int Compare(string strA, string strB, CultureInfo culture, CompareOptions options)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			Contract.EndContractBlock();

			return culture.CompareInfo.Compare(strA, strB, options);
		}



		// Provides a culture-correct string comparison. strA is compared to strB
		// to determine whether it is lexicographically less, equal, or greater, and then a
		// negative integer, 0, or a positive integer is returned; respectively.
		// The case-sensitive option is set by ignoreCase, and the culture is set
		// by culture
		//
		[Pure]
		public static int Compare(string strA, string strB, bool ignoreCase, CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			Contract.EndContractBlock();

			return ignoreCase
				? culture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase)
				: culture.CompareInfo.Compare(strA, strB, CompareOptions.None);
		}

		// Determines whether two string regions match.  The substring of strA beginning
		// at indexA of length count is compared with the substring of strB
		// beginning at indexB of the same length.
		//
		[Pure]
		public static int Compare(string strA, int indexA, string strB, int indexB, int length)
		{
			int lengthA = length;
			int lengthB = length;

			if (strA != null)
			{
				if (strA.Length - indexA < lengthA)
				{
					lengthA = strA.Length - indexA;
				}
			}

			if (strB != null)
			{
				if (strB.Length - indexB < lengthB)
				{
					lengthB = strB.Length - indexB;
				}
			}
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.None);
		}


		// Determines whether two string regions match.  The substring of strA beginning
		// at indexA of length count is compared with the substring of strB
		// beginning at indexB of the same length.  Case sensitivity is determined by the ignoreCase boolean.
		//
		[Pure]
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, bool ignoreCase)
		{
			int lengthA = length;
			int lengthB = length;

			if (strA != null)
			{
				if (strA.Length - indexA < lengthA)
				{
					lengthA = strA.Length - indexA;
				}
			}

			if (strB != null)
			{
				if (strB.Length - indexB < lengthB)
				{
					lengthB = strB.Length - indexB;
				}
			}

			return ignoreCase
				? CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.IgnoreCase)
				: CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.None);
		}

		// Determines whether two string regions match.  The substring of strA beginning
		// at indexA of length length is compared with the substring of strB
		// beginning at indexB of the same length.  Case sensitivity is determined by the ignoreCase boolean,
		// and the culture is set by culture.
		//
		[Pure]
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, bool ignoreCase, CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			Contract.EndContractBlock();

			int lengthA = length;
			int lengthB = length;

			if (strA != null)
			{
				if (strA.Length - indexA < lengthA)
				{
					lengthA = strA.Length - indexA;
				}
			}

			if (strB != null)
			{
				if (strB.Length - indexB < lengthB)
				{
					lengthB = strB.Length - indexB;
				}
			}

			return ignoreCase
				? culture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.IgnoreCase)
				: culture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.None);
		}


		// Determines whether two string regions match.  The substring of strA beginning
		// at indexA of length length is compared with the substring of strB
		// beginning at indexB of the same length.
		//
		[Pure]
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, CultureInfo culture, CompareOptions options)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			Contract.EndContractBlock();

			int lengthA = length;
			int lengthB = length;

			if (strA != null)
			{
				if (strA.Length - indexA < lengthA)
				{
					lengthA = strA.Length - indexA;
				}
			}

			if (strB != null)
			{
				if (strB.Length - indexB < lengthB)
				{
					lengthB = strB.Length - indexB;
				}
			}

			return culture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, options);
		}

		[Pure]
		[System.Security.SecuritySafeCritical]  // auto-generated
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, StringComparison comparisonType)
		{
			if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
			{
				throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
			Contract.EndContractBlock();

			if (strA == null || strB == null)
			{
				if (strA == (object)strB)
				{ //they're both null;
					return 0;
				}

				return (strA == null) ? -1 : 1; //-1 if A is null, 1 if B is null.
			}

			// @
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length",
													  Environment.GetResourceString("ArgumentOutOfRange_NegativeLength"));
			}

			if (indexA < 0)
			{
				throw new ArgumentOutOfRangeException("indexA",
													 Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}

			if (indexB < 0)
			{
				throw new ArgumentOutOfRangeException("indexB",
													 Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}

			if (strA.Length - indexA < 0)
			{
				throw new ArgumentOutOfRangeException("indexA",
													  Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}

			if (strB.Length - indexB < 0)
			{
				throw new ArgumentOutOfRangeException("indexB",
													  Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}

			if ((length == 0) ||
				((strA == strB) && (indexA == indexB)))
			{
				return 0;
			}

			int lengthA = length;
			int lengthB = length;

			if (strA != null)
			{
				if (strA.Length - indexA < lengthA)
				{
					lengthA = strA.Length - indexA;
				}
			}

			if (strB != null)
			{
				if (strB.Length - indexB < lengthB)
				{
					lengthB = strB.Length - indexB;
				}
			}

			switch (comparisonType)
			{
				case StringComparison.CurrentCulture:
					return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.None);

				case StringComparison.CurrentCultureIgnoreCase:
					return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.IgnoreCase);

				case StringComparison.InvariantCulture:
					return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.None);

				case StringComparison.InvariantCultureIgnoreCase:
					return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.IgnoreCase);

				case StringComparison.Ordinal:
					// 
					return nativeCompareOrdinalEx(strA, indexA, strB, indexB, length);

				case StringComparison.OrdinalIgnoreCase:
					return TextInfo.CompareOrdinalIgnoreCaseEx(strA, indexA, strB, indexB, lengthA, lengthB);

				default:
					throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"));
			}

		}

		// Compares this object to another object, returning an integer that
		// indicates the relationship. This method returns a value less than 0 if this is less than value, 0
		// if this is equal to value, or a value greater than 0
		// if this is greater than value.  Strings are considered to be
		// greater than all non-String objects.  Note that this means sorted 
		// arrays would contain nulls, other objects, then Strings in that order.
		//
		[Pure]
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}

			return !(value is string)
				? throw new ArgumentException(Environment.GetResourceString("Arg_MustBeString"))
				: string.Compare(this, (string)value, StringComparison.CurrentCulture);
		}

		// Determines the sorting relation of StrB to the current instance.
		//
		[Pure]
		public int CompareTo(string strB)
		{
			return strB == null ? 1 : CultureInfo.CurrentCulture.CompareInfo.Compare(this, strB, 0);
		}

		// Compares strA and strB using an ordinal (code-point) comparison.
		//
		[Pure]
		public static int CompareOrdinal(string strA, string strB)
		{
			if (strA == (object)strB)
			{
				return 0;
			}

			//they can't both be null;
			if (strA == null)
			{
				return -1;
			}

			if (strB == null)
			{
				return 1;
			}

			// Most common case, first character is different.
			if ((strA.m_firstChar - strB.m_firstChar) != 0)
			{
				return strA.m_firstChar - strB.m_firstChar;
			}

			// 
			return CompareOrdinalHelper(strA, strB);
		}


		// Compares strA and strB using an ordinal (code-point) comparison.
		//
		[Pure]
		[System.Security.SecuritySafeCritical]  // auto-generated
		public static int CompareOrdinal(string strA, int indexA, string strB, int indexB, int length)
		{
			if (strA == null || strB == null)
			{
				if (strA == (object)strB)
				{ //they're both null;
					return 0;
				}

				return (strA == null) ? -1 : 1; //-1 if A is null, 1 if B is null.
			}

			return nativeCompareOrdinalEx(strA, indexA, strB, indexB, length);
		}


		[Pure]
		public bool Contains(string value)
		{
			return IndexOf(value, StringComparison.Ordinal) >= 0;
		}

		// Determines whether a specified string is a suffix of the the current instance.
		//
		// The case-sensitive and culture-sensitive option is set by options,
		// and the default culture is used.
		//        
		[Pure]
		public bool EndsWith(string value)
		{
			return EndsWith(value, StringComparison.CurrentCulture);
		}

		[Pure]
		[System.Security.SecuritySafeCritical]  // auto-generated
		[ComVisible(false)]
		public bool EndsWith(string value, StringComparison comparisonType)
		{
			if (value is null)
			{
				throw new ArgumentNullException("value");
			}

			if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
			{
				throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
			Contract.EndContractBlock();

			if (this == (object)value)
			{
				return true;
			}

			if (value.Length == 0)
			{
				return true;
			}

			switch (comparisonType)
			{
				case StringComparison.CurrentCulture:
					return CultureInfo.CurrentCulture.CompareInfo.IsSuffix(this, value, CompareOptions.None);

				case StringComparison.CurrentCultureIgnoreCase:
					return CultureInfo.CurrentCulture.CompareInfo.IsSuffix(this, value, CompareOptions.IgnoreCase);

				case StringComparison.InvariantCulture:
					return CultureInfo.InvariantCulture.CompareInfo.IsSuffix(this, value, CompareOptions.None);

				case StringComparison.InvariantCultureIgnoreCase:
					return CultureInfo.InvariantCulture.CompareInfo.IsSuffix(this, value, CompareOptions.IgnoreCase);

				case StringComparison.Ordinal:
					return Length >= value.Length && (nativeCompareOrdinalEx(this, Length - value.Length, value, 0, value.Length) == 0);

				case StringComparison.OrdinalIgnoreCase:
					return Length >= value.Length && (TextInfo.CompareOrdinalIgnoreCaseEx(this, Length - value.Length, value, 0, value.Length, value.Length) == 0);

				default:
					throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
		}

		[Pure]
		public bool EndsWith(string value, bool ignoreCase, CultureInfo culture)
		{
			if (null == value)
			{
				throw new ArgumentNullException("value");
			}
			Contract.EndContractBlock();

			if (this == (object)value)
			{
				return true;
			}

			CultureInfo referenceCulture = culture == null ? CultureInfo.CurrentCulture : culture;

			return referenceCulture.CompareInfo.IsSuffix(this, value, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
		}

		[Pure]
		internal bool EndsWith(char value)
		{
			int thisLen = Length;
			if (thisLen != 0)
			{
				if (this[thisLen - 1] == value)
				{
					return true;
				}
			}
			return false;
		}


		// Returns the index of the first occurance of value in the current instance.
		// The search starts at startIndex and runs thorough the next count characters.
		//
		[Pure]
		public int IndexOf(char value)
		{
			return IndexOf(value, 0, Length);
		}

		[Pure]
		public int IndexOf(char value, int startIndex)
		{
			return IndexOf(value, startIndex, Length - startIndex);
		}

		[Pure]
		[System.Security.SecuritySafeCritical]  // auto-generated
		[ResourceExposure(ResourceScope.None)]
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public extern int IndexOf(char value, int startIndex, int count);

		// Returns the index of the first occurance of any character in value in the current instance.
		// The search starts at startIndex and runs to endIndex-1. [startIndex,endIndex).
		//
		[Pure]
		public int IndexOfAny(char[] anyOf)
		{
			return IndexOfAny(anyOf, 0, Length);
		}

		[Pure]
		public int IndexOfAny(char[] anyOf, int startIndex)
		{
			return IndexOfAny(anyOf, startIndex, Length - startIndex);
		}

		[Pure]
		[System.Security.SecuritySafeCritical]  // auto-generated
		[ResourceExposure(ResourceScope.None)]
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public extern int IndexOfAny(char[] anyOf, int startIndex, int count);


		// Determines the position within this string of the first occurence of the specified
		// string, according to the specified search criteria.  The search begins at
		// the first character of this string, it is case-sensitive and ordinal (code-point)
		// comparison is used.
		//
		[Pure]
		public int IndexOf(string value)
		{
			return IndexOf(value, StringComparison.CurrentCulture);
		}

		// Determines the position within this string of the first occurence of the specified
		// string, according to the specified search criteria.  The search begins at
		// startIndex, it is case-sensitive and ordinal (code-point) comparison is used.
		//
		[Pure]
		public int IndexOf(string value, int startIndex)
		{
			return IndexOf(value, startIndex, StringComparison.CurrentCulture);
		}

		// Determines the position within this string of the first occurence of the specified
		// string, according to the specified search criteria.  The search begins at
		// startIndex, ends at endIndex and ordinal (code-point) comparison is used.
		//
		[Pure]
		public int IndexOf(string value, int startIndex, int count)
		{
			if (startIndex < 0 || startIndex > Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}

			if (count < 0 || count > Length - startIndex)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
			}
			Contract.EndContractBlock();

			return IndexOf(value, startIndex, count, StringComparison.CurrentCulture);
		}

		[Pure]
		public int IndexOf(string value, StringComparison comparisonType)
		{
			return IndexOf(value, 0, Length, comparisonType);
		}

		[Pure]
		public int IndexOf(string value, int startIndex, StringComparison comparisonType)
		{
			return IndexOf(value, startIndex, Length - startIndex, comparisonType);
		}

		[Pure]
		[System.Security.SecuritySafeCritical]
		public int IndexOf(string value, int startIndex, int count, StringComparison comparisonType)
		{
			// Validate inputs
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			if (startIndex < 0 || startIndex > Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}

			if (count < 0 || startIndex > Length - count)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
			}

			Contract.EndContractBlock();

			switch (comparisonType)
			{
				case StringComparison.CurrentCulture:
					return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.None);

				case StringComparison.CurrentCultureIgnoreCase:
					return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);

				case StringComparison.InvariantCulture:
					return CultureInfo.InvariantCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.None);

				case StringComparison.InvariantCultureIgnoreCase:
					return CultureInfo.InvariantCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);

				case StringComparison.Ordinal:
					return CultureInfo.InvariantCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.Ordinal);

				case StringComparison.OrdinalIgnoreCase:
					return value.IsAscii() && IsAscii()
						? CultureInfo.InvariantCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase)
						: TextInfo.IndexOfStringOrdinalIgnoreCase(this, value, startIndex, count);

				default:
					throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
		}

		// Returns the index of the last occurance of value in the current instance.
		// The search starts at startIndex and runs to endIndex. [startIndex,endIndex].
		// The character at position startIndex is included in the search.  startIndex is the larger
		// index within the string.
		//
		[Pure]
		public int LastIndexOf(char value)
		{
			return LastIndexOf(value, Length - 1, Length);
		}

		[Pure]
		public int LastIndexOf(char value, int startIndex)
		{
			return LastIndexOf(value, startIndex, startIndex + 1);
		}

		[Pure]
		[System.Security.SecuritySafeCritical]  // auto-generated
		[ResourceExposure(ResourceScope.None)]
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public extern int LastIndexOf(char value, int startIndex, int count);

		// Returns the index of the last occurance of any character in value in the current instance.
		// The search starts at startIndex and runs to endIndex. [startIndex,endIndex].
		// The character at position startIndex is included in the search.  startIndex is the larger
		// index within the string.
		//

		//ForceInline ... Jit can't recognize String.get_Length to determine that this is "fluff"
		[Pure]
		public int LastIndexOfAny(char[] anyOf)
		{
			return LastIndexOfAny(anyOf, Length - 1, Length);
		}

		//ForceInline ... Jit can't recognize String.get_Length to determine that this is "fluff"
		[Pure]
		public int LastIndexOfAny(char[] anyOf, int startIndex)
		{
			return LastIndexOfAny(anyOf, startIndex, startIndex + 1);
		}

		[Pure]
		[System.Security.SecuritySafeCritical]  // auto-generated
		[ResourceExposure(ResourceScope.None)]
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public extern int LastIndexOfAny(char[] anyOf, int startIndex, int count);


		// Returns the index of the last occurance of any character in value in the current instance.
		// The search starts at startIndex and runs to endIndex. [startIndex,endIndex].
		// The character at position startIndex is included in the search.  startIndex is the larger
		// index within the string.
		//
		[Pure]
		public int LastIndexOf(string value)
		{
			return LastIndexOf(value, Length - 1, Length, StringComparison.CurrentCulture);
		}

		[Pure]
		public int LastIndexOf(string value, int startIndex)
		{
			return LastIndexOf(value, startIndex, startIndex + 1, StringComparison.CurrentCulture);
		}

		[Pure]
		public int LastIndexOf(string value, int startIndex, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
			}
			Contract.EndContractBlock();

			return LastIndexOf(value, startIndex, count, StringComparison.CurrentCulture);
		}

		[Pure]
		public int LastIndexOf(string value, StringComparison comparisonType)
		{
			return LastIndexOf(value, Length - 1, Length, comparisonType);
		}

		[Pure]
		public int LastIndexOf(string value, int startIndex, StringComparison comparisonType)
		{
			return LastIndexOf(value, startIndex, startIndex + 1, comparisonType);
		}

		[Pure]
		[System.Security.SecuritySafeCritical]
		public int LastIndexOf(string value, int startIndex, int count, StringComparison comparisonType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			Contract.EndContractBlock();

			// Special case for 0 length input strings
			if (Length == 0 && (startIndex == -1 || startIndex == 0))
			{
				return (value.Length == 0) ? 0 : -1;
			}

			// Now after handling empty strings, make sure we're not out of range
			if (startIndex < 0 || startIndex > Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}

			// Make sure that we allow startIndex == this.Length
			if (startIndex == Length)
			{
				startIndex--;
				if (count > 0)
				{
					count--;
				}

				// If we are looking for nothing, just return 0
				if (value.Length == 0 && count >= 0 && startIndex - count + 1 >= 0)
				{
					return startIndex;
				}
			}

			// 2nd half of this also catches when startIndex == MAXINT, so MAXINT - 0 + 1 == -1, which is < 0.
			if (count < 0 || startIndex - count + 1 < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
			}

			switch (comparisonType)
			{
				case StringComparison.CurrentCulture:
					return CultureInfo.CurrentCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.None);

				case StringComparison.CurrentCultureIgnoreCase:
					return CultureInfo.CurrentCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);

				case StringComparison.InvariantCulture:
					return CultureInfo.InvariantCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.None);

				case StringComparison.InvariantCultureIgnoreCase:
					return CultureInfo.InvariantCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
				case StringComparison.Ordinal:
					return CultureInfo.InvariantCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.Ordinal);

				case StringComparison.OrdinalIgnoreCase:
					return value.IsAscii() && IsAscii()
						? CultureInfo.InvariantCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase)
						: TextInfo.LastIndexOfStringOrdinalIgnoreCase(this, value, startIndex, count);
				default:
					throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
		}

		//
		//
		[Pure]
		public string PadLeft(int totalWidth)
		{
			return PadHelper(totalWidth, ' ', false);
		}

		[Pure]
		public string PadLeft(int totalWidth, char paddingChar)
		{
			return PadHelper(totalWidth, paddingChar, false);
		}

		[Pure]
		public string PadRight(int totalWidth)
		{
			return PadHelper(totalWidth, ' ', true);
		}

		[Pure]
		public string PadRight(int totalWidth, char paddingChar)
		{
			return PadHelper(totalWidth, paddingChar, true);
		}


		[System.Security.SecuritySafeCritical]  // auto-generated
		[ResourceExposure(ResourceScope.None)]
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern string PadHelper(int totalWidth, char paddingChar, bool isRightPadded);

		// Determines whether a specified string is a prefix of the current instance
		//
		[Pure]
		public bool StartsWith(string value)
		{
			if (value is null)
			{
				throw new ArgumentNullException("value");
			}
			Contract.EndContractBlock();
			return StartsWith(value, StringComparison.CurrentCulture);
		}

		[Pure]
		[System.Security.SecuritySafeCritical]  // auto-generated
		[ComVisible(false)]
		public bool StartsWith(string value, StringComparison comparisonType)
		{
			if (value is null)
			{
				throw new ArgumentNullException("value");
			}

			if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
			{
				throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
			Contract.EndContractBlock();

			if (this == (object)value)
			{
				return true;
			}

			if (value.Length == 0)
			{
				return true;
			}

			switch (comparisonType)
			{
				case StringComparison.CurrentCulture:
					return CultureInfo.CurrentCulture.CompareInfo.IsPrefix(this, value, CompareOptions.None);

				case StringComparison.CurrentCultureIgnoreCase:
					return CultureInfo.CurrentCulture.CompareInfo.IsPrefix(this, value, CompareOptions.IgnoreCase);

				case StringComparison.InvariantCulture:
					return CultureInfo.InvariantCulture.CompareInfo.IsPrefix(this, value, CompareOptions.None);

				case StringComparison.InvariantCultureIgnoreCase:
					return CultureInfo.InvariantCulture.CompareInfo.IsPrefix(this, value, CompareOptions.IgnoreCase);

				case StringComparison.Ordinal:
					if (Length < value.Length)
					{
						return false;
					}
					return nativeCompareOrdinalEx(this, 0, value, 0, value.Length) == 0;

				case StringComparison.OrdinalIgnoreCase:
					if (Length < value.Length)
					{
						return false;
					}

					return TextInfo.CompareOrdinalIgnoreCaseEx(this, 0, value, 0, value.Length, value.Length) == 0;

				default:
					throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
		}

		[Pure]
		public bool StartsWith(string value, bool ignoreCase, CultureInfo culture)
		{
			if (null == value)
			{
				throw new ArgumentNullException("value");
			}
			Contract.EndContractBlock();

			if (this == (object)value)
			{
				return true;
			}

			CultureInfo referenceCulture = culture == null ? CultureInfo.CurrentCulture : culture;

			return referenceCulture.CompareInfo.IsPrefix(this, value, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
		}

		// Creates a copy of this string in lower case.
		[Pure]
		public string ToLower()
		{
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.EndContractBlock();
			return this.ToLower(CultureInfo.CurrentCulture);
		}

		// Creates a copy of this string in lower case.  The culture is set by culture.
		[Pure]
		public string ToLower(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.EndContractBlock();
			return culture.TextInfo.ToLower(this);
		}

		// Creates a copy of this string in lower case based on invariant culture.
		[Pure]
		public string ToLowerInvariant()
		{
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.EndContractBlock();
			return this.ToLower(CultureInfo.InvariantCulture);
		}

		// Creates a copy of this string in upper case.
		[Pure]
		public string ToUpper()
		{
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.EndContractBlock();
			return this.ToUpper(CultureInfo.CurrentCulture);
		}


		// Creates a copy of this string in upper case.  The culture is set by culture.
		[Pure]
		public string ToUpper(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.EndContractBlock();
			return culture.TextInfo.ToUpper(this);
		}


		//Creates a copy of this string in upper case based on invariant culture.
		[Pure]
		public string ToUpperInvariant()
		{
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.EndContractBlock();
			return this.ToUpper(CultureInfo.InvariantCulture);
		}


		// Returns this string.
		public override string ToString()
		{
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.EndContractBlock();
			return this;
		}

		public string ToString(IFormatProvider provider)
		{
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.EndContractBlock();
			return this;
		}

		// Method required for the ICloneable interface.
		// There's no point in cloning a string since they're immutable, so we simply return this.
		public object Clone()
		{
			Contract.Ensures(Contract.Result<object>() != null);
			Contract.EndContractBlock();
			return this;
		}

		private static bool IsBOMWhitespace(char c)
		{
#if FEATURE_LEGACYNETCF
			if (CompatibilitySwitches.IsAppEarlierThanWindowsPhone8 && c == '\xFEFF')
			{
				// Dev11 450846 quirk:
				// NetCF treats the BOM as a whitespace character when performing trim operations.
				return true;
			}
			else
#endif
			{
				return false;
			}
		}

		// Trims the whitespace from both ends of the string.  Whitespace is defined by
		// Char.IsWhiteSpace.
		//
		[Pure]
		public string Trim()
		{
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.EndContractBlock();

			return TrimHelper(TrimBoth);
		}


		[System.Security.SecuritySafeCritical]  // auto-generated
		private string TrimHelper(int trimType)
		{
			//end will point to the first non-trimmed character on the right
			//start will point to the first non-trimmed character on the Left
			int end = Length - 1;
			int start = 0;

			//Trim specified characters.
			if (trimType != TrimTail)
			{
				for (start = 0; start < Length; start++)
				{
					if (!char.IsWhiteSpace(this[start]) && !IsBOMWhitespace(this[start]))
					{
						break;
					}
				}
			}

			if (trimType != TrimHead)
			{
				for (end = Length - 1; end >= start; end--)
				{
					if (!char.IsWhiteSpace(this[end]) && !IsBOMWhitespace(this[start]))
					{
						break;
					}
				}
			}

			return CreateTrimmedString(start, end);
		}


		[System.Security.SecuritySafeCritical]  // auto-generated
		private string TrimHelper(char[] trimChars, int trimType)
		{
			//end will point to the first non-trimmed character on the right
			//start will point to the first non-trimmed character on the Left
			int end = Length - 1;
			int start = 0;

			//Trim specified characters.
			if (trimType != TrimTail)
			{
				for (start = 0; start < Length; start++)
				{
					int i = 0;
					char ch = this[start];
					for (i = 0; i < trimChars.Length; i++)
					{
						if (trimChars[i] == ch)
						{
							break;
						}
					}
					if (i == trimChars.Length)
					{ // the character is not white space
						break;
					}
				}
			}

			if (trimType != TrimHead)
			{
				for (end = Length - 1; end >= start; end--)
				{
					int i = 0;
					char ch = this[end];
					for (i = 0; i < trimChars.Length; i++)
					{
						if (trimChars[i] == ch)
						{
							break;
						}
					}
					if (i == trimChars.Length)
					{ // the character is not white space
						break;
					}
				}
			}

			return CreateTrimmedString(start, end);
		}


		[System.Security.SecurityCritical]  // auto-generated
		private string CreateTrimmedString(int start, int end)
		{
			//Create a new STRINGREF and initialize it from the range determined above.
			int len = end - start + 1;
			if (len == Length)
			{
				// Don't allocate a new string as the trimmed string has not changed.
				return this;
			}

			return len == 0 ? String.Empty : InternalSubString(start, len);
		}

		[System.Security.SecuritySafeCritical]  // auto-generated
		public string Insert(int startIndex, string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			if (startIndex < 0 || startIndex > Length)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}

			Contract.Ensures(Contract.Result<string>() != null);
			Contract.Ensures(Contract.Result<string>().Length == Length + value.Length);
			Contract.EndContractBlock();
			int oldLength = Length;
			int insertLength = value.Length;
			// In case this computation overflows, newLength will be negative and FastAllocateString throws OutOfMemoryException
			int newLength = oldLength + insertLength;
			if (newLength == 0)
			{
				return String.Empty;
			}

			string result = FastAllocateString(newLength);
			unsafe
			{
				fixed (char* srcThis = &m_firstChar)
				{
					fixed (char* srcInsert = &value.m_firstChar)
					{
						fixed (char* dst = &result.m_firstChar)
						{
							wstrcpy(dst, srcThis, startIndex);
							wstrcpy(dst + startIndex, srcInsert, insertLength);
							wstrcpy(dst + startIndex + insertLength, srcThis + startIndex, oldLength - startIndex);
						}
					}
				}
			}
			return result;
		}


		// This method contains the same functionality as StringBuilder Replace. The only difference is that
		// a new String has to be allocated since Strings are immutable

		[System.Security.SecuritySafeCritical]  // auto-generated
		public string Remove(int startIndex, int count)
		{
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex",
					Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}

			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count",
					Environment.GetResourceString("ArgumentOutOfRange_NegativeCount"));
			}

			if (count > Length - startIndex)
			{
				throw new ArgumentOutOfRangeException("count",
					Environment.GetResourceString("ArgumentOutOfRange_IndexCount"));
			}

			Contract.Ensures(Contract.Result<string>() != null);
			Contract.Ensures(Contract.Result<string>().Length == Length - count);
			Contract.EndContractBlock();
			int newLength = Length - count;
			if (newLength == 0)
			{
				return String.Empty;
			}

			string result = FastAllocateString(newLength);
			unsafe
			{
				fixed (char* src = &m_firstChar)
				{
					fixed (char* dst = &result.m_firstChar)
					{
						wstrcpy(dst, src, startIndex);
						wstrcpy(dst + startIndex, src + startIndex + count, newLength - startIndex);
					}
				}
			}
			return result;
		}

		// a remove that just takes a startindex. 
		public string Remove(int startIndex)
		{
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex",
						Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}

			if (startIndex >= Length)
			{
				throw new ArgumentOutOfRangeException("startIndex",
						Environment.GetResourceString("ArgumentOutOfRange_StartIndexLessThanLength"));
			}

			Contract.Ensures(Contract.Result<string>() != null);
			Contract.EndContractBlock();

			return Substring(0, startIndex);
		}

		[System.Security.SecuritySafeCritical]  // auto-generated
		public static unsafe string Copy(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.EndContractBlock();

			int length = str.Length;

			string result = FastAllocateString(length);

			fixed (char* dest = &result.m_firstChar)
			fixed (char* src = &str.m_firstChar)
			{
				wstrcpy(dest, src, length);
			}
			return result;
		}

		public static string Concat(object arg0)
		{
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.EndContractBlock();

			return arg0 == null ? String.Empty : arg0.ToString();
		}

		public static string Concat(object arg0, object arg1)
		{
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.EndContractBlock();

			arg0 ??= String.Empty;

			arg1 ??= String.Empty;
			return Concat(arg0.ToString(), arg1.ToString());
		}

		public static string Concat(object arg0, object arg1, object arg2)
		{
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.EndContractBlock();

			arg0 ??= String.Empty;

			arg1 ??= String.Empty;

			arg2 ??= String.Empty;

			return Concat(arg0.ToString(), arg1.ToString(), arg2.ToString());
		}


		public static string Concat(params object[] args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.EndContractBlock();

			string[] sArgs = new string[args.Length];
			int totalLength = 0;

			for (int i = 0; i < args.Length; i++)
			{
				object value = args[i];
				sArgs[i] = (value == null) ? String.Empty : value.ToString();
				if (sArgs[i] == null)
				{
					sArgs[i] = String.Empty; // value.ToString() above could have returned null
				}

				totalLength += sArgs[i].Length;
				// check for overflow
				if (totalLength < 0)
				{
					throw new OutOfMemoryException();
				}
			}
			return ConcatArray(sArgs, totalLength);
		}



		[System.Security.SecuritySafeCritical]  // auto-generated
		public static string Concat(string str0, string str1)
		{
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.Ensures(Contract.Result<string>().Length ==
				(str0 == null ? 0 : str0.Length) +
				(str1 == null ? 0 : str1.Length));
			Contract.EndContractBlock();

			if (IsNullOrEmpty(str0))
			{
				return IsNullOrEmpty(str1) ? String.Empty : str1;
			}

			if (IsNullOrEmpty(str1))
			{
				return str0;
			}

			int str0Length = str0.Length;

			string result = FastAllocateString(str0Length + str1.Length);

			FillStringChecked(result, 0, str0);
			FillStringChecked(result, str0Length, str1);

			return result;
		}

		[System.Security.SecuritySafeCritical]  // auto-generated
		public static string Concat(string str0, string str1, string str2)
		{
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.Ensures(Contract.Result<string>().Length ==
				(str0 == null ? 0 : str0.Length) +
				(str1 == null ? 0 : str1.Length) +
				(str2 == null ? 0 : str2.Length));
			Contract.EndContractBlock();

			if (str0 == null && str1 == null && str2 == null)
			{
				return String.Empty;
			}

			str0 ??= String.Empty;

			str1 ??= String.Empty;

			str2 ??= String.Empty;

			int totalLength = str0.Length + str1.Length + str2.Length;

			string result = FastAllocateString(totalLength);
			FillStringChecked(result, 0, str0);
			FillStringChecked(result, str0.Length, str1);
			FillStringChecked(result, str0.Length + str1.Length, str2);

			return result;
		}

		[System.Security.SecuritySafeCritical]  // auto-generated
		public static string Concat(string str0, string str1, string str2, string str3)
		{
			Contract.Ensures(Contract.Result<string>() != null);
			Contract.Ensures(Contract.Result<string>().Length ==
				(str0 == null ? 0 : str0.Length) +
				(str1 == null ? 0 : str1.Length) +
				(str2 == null ? 0 : str2.Length) +
				(str3 == null ? 0 : str3.Length));
			Contract.EndContractBlock();

			if (str0 == null && str1 == null && str2 == null && str3 == null)
			{
				return String.Empty;
			}

			str0 ??= String.Empty;

			str1 ??= String.Empty;

			str2 ??= String.Empty;

			str3 ??= String.Empty;

			int totalLength = str0.Length + str1.Length + str2.Length + str3.Length;

			string result = FastAllocateString(totalLength);
			FillStringChecked(result, 0, str0);
			FillStringChecked(result, str0.Length, str1);
			FillStringChecked(result, str0.Length + str1.Length, str2);
			FillStringChecked(result, str0.Length + str1.Length + str2.Length, str3);

			return result;
		}

		[System.Security.SecuritySafeCritical]  // auto-generated
		private static string ConcatArray(string[] values, int totalLength)
		{
			string result = FastAllocateString(totalLength);
			int currPos = 0;

			for (int i = 0; i < values.Length; i++)
			{
				Contract.Assert(currPos <= totalLength - values[i].Length,
								"[String.ConcatArray](currPos <= totalLength - values[i].Length)");

				FillStringChecked(result, currPos, values[i]);
				currPos += values[i].Length;
			}

			return result;
		}

		public static string Concat(params string[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}

			Contract.Ensures(Contract.Result<string>() != null);
			// Spec#: Consider a postcondition saying the length of this string == the sum of each string in array
			Contract.EndContractBlock();
			int totalLength = 0;

			// Always make a copy to prevent changing the array on another thread.
			string[] internalValues = new string[values.Length];

			for (int i = 0; i < values.Length; i++)
			{
				string value = values[i];
				internalValues[i] = (value == null) ? String.Empty : value;
				totalLength += internalValues[i].Length;
				// check for overflow
				if (totalLength < 0)
				{
					throw new OutOfMemoryException();
				}
			}

			return ConcatArray(internalValues, totalLength);
		}
	}

	[Flags]
	public enum StringSplitOptions
	{
		None = 0,
		RemoveEmptyEntries = 1
	}
}*/