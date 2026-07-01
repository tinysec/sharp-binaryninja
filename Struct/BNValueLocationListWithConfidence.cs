using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNValueLocationListWithConfidence
	{
		/// <summary>
		/// BNValueLocation* locations
		/// </summary>
		internal IntPtr locations;

		/// <summary>
		/// size_t count
		/// </summary>
		internal ulong count;

		/// <summary>
		/// uint8_t confidence
		/// </summary>
		internal byte confidence;
	}
}
