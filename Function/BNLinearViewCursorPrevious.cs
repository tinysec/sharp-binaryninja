using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNLinearViewCursorPrevious(BNLinearViewCursor* cursor)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNLinearViewCursorPrevious"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNLinearViewCursorPrevious(
			
			// BNLinearViewCursor* cursor
		    IntPtr cursor  
		);
	}
}