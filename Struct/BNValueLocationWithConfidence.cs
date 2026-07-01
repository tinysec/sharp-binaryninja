using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNValueLocationWithConfidence
	{
		/// <summary>
		/// BNValueLocation location
		/// </summary>
		internal BNValueLocation location;

		/// <summary>
		/// uint8_t confidence
		/// </summary>
		internal byte confidence;
	}
}
