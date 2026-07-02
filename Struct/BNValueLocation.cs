using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNValueLocation
	{
		/// <summary>
		/// size_t count
		/// </summary>
		internal ulong count;

		/// <summary>
		/// BNValueLocationComponent* components
		/// </summary>
		internal IntPtr components;

		/// <summary>
		/// bool indirect
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool indirect;

		/// <summary>
		/// bool returnedPointerValid
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool returnedPointerValid;

		/// <summary>
		/// BNVariable returnedPointer
		/// </summary>
		internal BNVariable returnedPointer;
	}
}
