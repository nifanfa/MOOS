using System.Runtime;
using MOOS;

internal unsafe class Program
{
	/// <summary>
	/// Compiler requires a static main method in class 'Program'
	/// 
	/// <![CDATA[
	/// Do not remove!
	/// ]]>
	/// </summary>
	private static void Main() { }

	/**
	 * Minimum memory is 1024MiB of RAM
	 * Memory Map:
	 * 256 MiB - 512MiB    System
	 * 512 MiB - ∞         Free to use
	 * <see cref="MOOS.Misc.EntryPoint"/>
	 */
	[RuntimeExport("KMain")]
	private static void KMain()
	{
		Console.Clear();
		Console.WriteLine("Welcome to MOOS! Type some text and press {ENTER} for it to be 'echoed' back, just like the linux 'cat' command");

		for (; ; )
		{
			string s = Console.ReadLine();
			Console.WriteLine(s);
		}
	}
}