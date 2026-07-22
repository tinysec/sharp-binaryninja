using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate IntPtr BNLineFormatterFormatLines(
			IntPtr context,
			IntPtr inputLines,
			ulong inputCount,
			IntPtr settings,
			IntPtr outputCount
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNLineFormatterFreeLines(
			IntPtr context,
			IntPtr lines,
			ulong count
		);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNCustomLineFormatter
	{
		/// <summary>
		/// void* context
		/// </summary>
		public IntPtr context;
		
		/// <summary>
		/// void** formatLines
		/// </summary>
		public IntPtr formatLines;
		
		/// <summary>
		/// void** freeLines
		/// </summary>
		public IntPtr freeLines;
	}

	public class CustomLineFormatter
    {
		public CustomLineFormatter() 
		{
		    
		}
    }
}
