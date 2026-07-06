using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSeekLinearViewCursorToCursorPathAndAddress(BNLinearViewCursor* cursor, BNLinearViewCursor* path, uint64_t addr)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSeekLinearViewCursorToCursorPathAndAddress"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSeekLinearViewCursorToCursorPathAndAddress(
			
			// BNLinearViewCursor* cursor
		    IntPtr cursor  , 
			
			// BNLinearViewCursor* path
		    IntPtr path  , 
			
			// uint64_t addr
		    ulong addr  
			
		);
	}
}