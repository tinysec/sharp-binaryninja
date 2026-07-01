using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNInlineDuringAnalysisWithConfidence
	{
		/// <summary>
		/// BNInlineDuringAnalysis value
		/// </summary>
		internal InlineDuringAnalysis value;

		/// <summary>
		/// uint8_t confidence
		/// </summary>
		internal byte confidence;
	}
}
