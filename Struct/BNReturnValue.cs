using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNReturnValue
	{
		/// <summary>
		/// BNType* type
		/// </summary>
		internal IntPtr type;

		/// <summary>
		/// uint8_t typeConfidence
		/// </summary>
		internal byte typeConfidence;

		/// <summary>
		/// bool defaultLocation
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool defaultLocation;

		/// <summary>
		/// BNValueLocation location
		/// </summary>
		internal BNValueLocation location;

		/// <summary>
		/// uint8_t locationConfidence
		/// </summary>
		internal byte locationConfidence;
	}
}
