using System;
using System.Runtime.InteropServices;


internal static unsafe class Platform {
	[DllImport("*")]
	public static extern IntPtr kmalloc(ulong size);

	[DllImport("*")]
	public static extern void kfree(IntPtr ptr);


	// Based on Nord (https://www.nordtheme.com/)
	static readonly uint[] ttyColours = new uint[]{
		0x002E3440,	// Black
		0x005E81AC,	// Blue
		0x00A3BE8C,	// Green
		0x0088C0D0,	// Cyan
		0x00BF616A,	// Red
		0x00B48EAD,	// Magenta
		0x00D08770,	// Yellow
		0x00D8DEE9,	// White
		0x004C566A,	// BrightBlack
		0x0081A1C1,	// BrightBlue
		0x00E3FECC,	// BrightGreen
		0x0088E0F0,	// BrightCyan
		0x00FFA1AA,	// BrightRed
		0x00F4CEED,	// BrightMagenta
		0x00EBCB8B,	// BrightYellow
		0x00ECEFF4	// BrightWhite
	};

	static uint[] ttyGlyphData = null;
	static uint ttyX, ttyY, ttyCols, ttyRows;
	static uint ttyBgValue = ttyColours[(int)ConsoleColor.Black], ttyFgValue = ttyColours[(int)ConsoleColor.White];


	public static IntPtr Allocate(ulong size)
		=> kmalloc(size);

	public static void Free(IntPtr ptr) {
		kfree(ptr);
	}

	public static unsafe void ZeroMemory(IntPtr ptr, ulong len) {
		var count = len / 8;
		var rem = len % 8;

		for (var i = 0U; i < count; i++)
			((ulong*)ptr)[i] = 0;

		for (var i = 0U; i < rem; i++)
			((byte*)ptr)[count + i] = 0;
	}

	public static unsafe void CopyMemory(IntPtr dst, IntPtr src, ulong len) {
		var count = len / 8;
		var rem = len % 8;

		for (var i = 0U; i < count; i++)
			((ulong*)dst)[i] = ((ulong*)src)[i];

		for (var i = 0U; i < rem; i++)
			((byte*)dst)[count + i] = ((byte*)src)[count + i];
	}
}