using System;
using System.Runtime.InteropServices;

namespace ModTools
{
	public class Core
	{
		[DllImport("kernel32.dll")]
		public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
	}
}
