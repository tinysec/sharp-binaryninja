using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSegmentIsAutoDefined(BNSegment* segment)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSegmentIsAutoDefined"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSegmentIsAutoDefined(
			
			// BNSegment* segment
		    IntPtr segment  
			
		);
	}
}