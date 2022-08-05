using Internal.Runtime.CompilerHelpers;

namespace System
{
	// The BitConverter class contains methods for
	// converting an array of bytes to one of the base data 
	// types, as well as for converting a base data type to an
	// array of bytes.
	// 
	// Only statics, does not need to be marked with the serializable attribute
	public static class BitConverter
	{

		// This field indicates the "endianess" of the architecture.
		// The value is set to true if the architecture is
		// little endian; false if it is big endian.
#if BIGENDIAN
	public static readonly bool IsLittleEndian /* = false */;
#else
		public static readonly bool IsLittleEndian = true;
#endif

		// Converts a byte into an array of bytes with length one.
		public static byte[] GetBytes(bool value)
		{
			byte[] r = new byte[1];
			r[0] = value ? (byte)bool.True : (byte)bool.False;
			return r;
		}

		// Converts a char into an array of bytes with length two.
		public static byte[] GetBytes(char value)
		{
			return GetBytes((short)value);
		}

		// Converts a short into an array of bytes with length
		// two.
		public static unsafe byte[] GetBytes(short value)
		{
			byte[] bytes = new byte[2];
			fixed (byte* b = bytes)
			{
				*(short*)b = value;
			}

			return bytes;
		}

		// Converts an int into an array of bytes with length 
		// four.
		public static unsafe byte[] GetBytes(int value)
		{
			byte[] bytes = new byte[4];
			fixed (byte* b = bytes)
			{
				*(int*)b = value;
			}

			return bytes;
		}

		// Converts a long into an array of bytes with length 
		// eight.
		public static unsafe byte[] GetBytes(long value)
		{
			byte[] bytes = new byte[8];
			fixed (byte* b = bytes)
			{
				*(long*)b = value;
			}

			return bytes;
		}

		// Converts an ushort into an array of bytes with
		// length two.

		public static byte[] GetBytes(ushort value)
		{
			return GetBytes((short)value);
		}

		// Converts an uint into an array of bytes with
		// length four.
		public static byte[] GetBytes(uint value)
		{
			return GetBytes((int)value);
		}

		// Converts an unsigned long into an array of bytes with
		// length eight.

		public static byte[] GetBytes(ulong value)
		{
			return GetBytes((long)value);
		}

		// Converts a float into an array of bytes with length 
		// four.

		public static unsafe byte[] GetBytes(float value)
		{
			return GetBytes(*(int*)&value);
		}

		// Converts a double into an array of bytes with length 
		// eight.
		public static unsafe byte[] GetBytes(double value)
		{
			return GetBytes(*(long*)&value);
		}

		// Converts an array of bytes into a char.  
		public static char ToChar(byte[] value, int startIndex)
		{
			if (value == null)
			{
				//ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}

			if ((uint)startIndex >= value.Length)
			{
				//ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}

			if (startIndex > value.Length - 2)
			{
				//ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			return (char)ToInt16(value, startIndex);
		}

		// Converts an array of bytes into a short.  
		public static unsafe short ToInt16(byte[] value, int startIndex)
		{
			if (value == null)
			{
				//ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}

			if ((uint)startIndex >= value.Length)
			{
				//ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}

			if (startIndex > value.Length - 2)
			{
				//ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}

			fixed (byte* pbyte = &value[startIndex])
			{
				return startIndex % 2 == 0
				? *(short*)pbyte
				: IsLittleEndian ? (short)((*pbyte) | (*(pbyte + 1) << 8)) : (short)((*pbyte << 8) | (*(pbyte + 1)));
			}

		}

		// Converts an array of bytes into an int.  
		public static unsafe int ToInt32(byte[] value, int startIndex)
		{
			if (value == null)
			{
				//ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}

			if ((uint)startIndex >= value.Length)
			{
				//ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (startIndex > value.Length - 4)
			{
				//ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}

			fixed (byte* pbyte = &value[startIndex])
			{
				return startIndex % 4 == 0
				? *(int*)pbyte
				: IsLittleEndian
				? (*pbyte) | (*(pbyte + 1) << 8) | (*(pbyte + 2) << 16) | (*(pbyte + 3) << 24)
				: (*pbyte << 24) | (*(pbyte + 1) << 16) | (*(pbyte + 2) << 8) | (*(pbyte + 3));
			}
		}

		// Converts an array of bytes into a long.
		public static unsafe long ToInt64(byte[] value, int startIndex)
		{
			if (value == null)
			{
				//ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}

			if ((uint)startIndex >= value.Length)
			{
				//ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (startIndex > value.Length - 8)
			{
				//ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}

			fixed (byte* pbyte = &value[startIndex])
			{
				if (startIndex % 8 == 0)
				{ // data is aligned 
					return *(long*)pbyte;
				} else
				{
					if (IsLittleEndian)
					{
						int i1 = (*pbyte) | (*(pbyte + 1) << 8) | (*(pbyte + 2) << 16) | (*(pbyte + 3) << 24);
						int i2 = (*(pbyte + 4)) | (*(pbyte + 5) << 8) | (*(pbyte + 6) << 16) | (*(pbyte + 7) << 24);
						return (uint)i1 | ((long)i2 << 32);
					} else
					{
						int i1 = (*pbyte << 24) | (*(pbyte + 1) << 16) | (*(pbyte + 2) << 8) | (*(pbyte + 3));
						int i2 = (*(pbyte + 4) << 24) | (*(pbyte + 5) << 16) | (*(pbyte + 6) << 8) | (*(pbyte + 7));
						return (uint)i2 | ((long)i1 << 32);
					}
				}
			}
		}


		// Converts an array of bytes into an ushort.
		// 
		public static ushort ToUInt16(byte[] value, int startIndex)
		{
			if (value == null)
			{
				//ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}

			if ((uint)startIndex >= value.Length)
			{
				//ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}

			if (startIndex > value.Length - 2)
			{
				//ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}

			return (ushort)ToInt16(value, startIndex);
		}

		// Converts an array of bytes into an uint.
		// 
		public static uint ToUInt32(byte[] value, int startIndex)
		{
			if (value == null)
			{
				//ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}

			if ((uint)startIndex >= value.Length)
			{
				//ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}

			if (startIndex > value.Length - 4)
			{
				//ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}

			return (uint)ToInt32(value, startIndex);
		}

		// Converts an array of bytes into an unsigned long.
		// 
		public static ulong ToUInt64(byte[] value, int startIndex)
		{
			if (value == null)
			{
				//ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}

			if ((uint)startIndex >= value.Length)
			{
				//ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}

			if (startIndex > value.Length - 8)
			{
				//ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}

			return (ulong)ToInt64(value, startIndex);
		}

		// Converts an array of bytes into a float.  
		public static unsafe float ToSingle(byte[] value, int startIndex)
		{
			if (value == null)
			{
				//ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}

			if ((uint)startIndex >= value.Length)
			{
				//ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}

			if (startIndex > value.Length - 4)
			{
				//ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}

			int val = ToInt32(value, startIndex);
			return *(float*)&val;
		}

		// Converts an array of bytes into a double.  
		public static unsafe double ToDouble(byte[] value, int startIndex)
		{
			if (value == null)
			{
				//ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}

			if ((uint)startIndex >= value.Length)
			{
				//ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}

			if (startIndex > value.Length - 8)
			{
				//ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}

			long val = ToInt64(value, startIndex);
			return *(double*)&val;
		}
		private static char GetHexValue(int i)
		{
			return i < 10 ? (char)(i + '0') : (char)(i - 10 + 'A');
		}

		// Converts an array of bytes into a String.  
		public static string ToString(byte[] value, int startIndex, int length)
		{
			/*
			if (value == null)
			{
				ThrowHelpers.ThrowArgumentNullException("value");
			}

			if (startIndex < 0 || (startIndex >= value.Length && startIndex > 0))
			{  // Don't throw for a 0 length array.
				ThrowHelpers.ThrowArgumentOutOfRangeException("startIndex", SR.ArgumentOutOfRange_StartIndex);
			}

			if (length < 0)
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("length", SR.ArgumentOutOfRange_GenericPositive);
			}

			if (startIndex > value.Length - length)
			{
				ThrowHelpers.ThrowArgumentException("value.Length, length, startIndex", SR.Arg_ArrayPlusOffTooSmall);
			}
			*/

			if (length == 0)
			{
				return string.Empty;
			}

			/*
			if (length > (int.MaxValue / 3))
			{
				// (Int32.MaxValue / 3) == 715,827,882 Bytes == 699 MB
				ThrowHelpers.ThrowArgumentOutOfRangeException("length", string.Format(SR.ArgumentOutOfRange_LengthTooLarge, int.MaxValue / 3));
			}
			*/

			int chArrayLength = length * 3;

			char[] chArray = new char[chArrayLength];
			int index = startIndex;
			for (int i = 0; i < chArrayLength; i += 3)
			{
				byte b = value[index++];
				chArray[i] = GetHexValue(b / 16);
				chArray[i + 1] = GetHexValue(b % 16);
				chArray[i + 2] = '-';
			}

			// We don't need the last '-' character
			return new string(chArray, 0, chArray.Length - 1);
		}

		// Converts an array of bytes into a String.  
		public static string ToString(byte[] value)
		{
			/*
			if (value == null)
			{
				ThrowHelpers.ThrowArgumentNullException("value");
			}
			*/

			return ToString(value, 0, value.Length);
		}

		// Converts an array of bytes into a String.  
		public static string ToString(byte[] value, int startIndex)
		{
			/*
			if (value == null)
			{
				ThrowHelpers.ThrowArgumentNullException("value");
			}
			*/

			return ToString(value, startIndex, value.Length - startIndex);
		}

		/*==================================ToBoolean===================================
		**Action:  Convert an array of bytes to a boolean value.  We treat this array 
		**         as if the first 4 bytes were an Int4 an operate on this value.
		**Returns: True if the Int4 value of the first 4 bytes is non-zero.
		**Arguments: value -- The byte array
		**           startIndex -- The position within the array.
		**Exceptions: See ToInt4.
		==============================================================================*/
		// Converts an array of bytes into a boolean.  
		public static bool ToBoolean(byte[] value, int startIndex)
		{
			/*
			if (value == null)
			{
				ThrowHelpers.ThrowArgumentNullException("value");
			}

			if (startIndex < 0)
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("startIndex", SR.ArgumentOutOfRange_NeedNonNegNum);
			}

			if (startIndex > value.Length - 1)
			{
				ThrowHelpers.ThrowArgumentOutOfRangeException("startIndex", SR.ArgumentOutOfRange_Index);
			}
			*/

			return value[startIndex] != 0;
		}

		public static unsafe long DoubleToInt64Bits(double value)
		{
			// If we're on a big endian machine, what should this method do?  You could argue for
			// either big endian or little endian, depending on whether you are writing to a file that
			// should be used by other programs on that processor, or for compatibility across multiple
			// formats.  Because this is ambiguous, we're excluding this from the Portable Library & Win8 Profile.
			// If we ever run on big endian machines, produce two versions where endianness is specified.
			return *(long*)&value;
		}

		public static unsafe double Int64BitsToDouble(long value)
		{
			// If we're on a big endian machine, what should this method do?  You could argue for
			// either big endian or little endian, depending on whether you are writing to a file that
			// should be used by other programs on that processor, or for compatibility across multiple
			// formats.  Because this is ambiguous, we're excluding this from the Portable Library & Win8 Profile.
			// If we ever run on big endian machines, produce two versions where endianness is specified.
			return *(double*)&value;
		}
	}
}