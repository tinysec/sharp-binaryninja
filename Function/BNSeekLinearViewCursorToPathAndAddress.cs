using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSeekLinearViewCursorToPathAndAddress(BNLinearViewCursor* cursor, BNLinearViewObjectIdentifier* ids, uint64_t count, uint64_t addr)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSeekLinearViewCursorToPathAndAddress"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSeekLinearViewCursorToPathAndAddress(
			
			// BNLinearViewCursor* cursor
		    IntPtr cursor  , 
			
			// BNLinearViewObjectIdentifier* ids
		    IntPtr ids  , 
			
			// uint64_t count
		    ulong count  , 
			
			// uint64_t addr
		    ulong addr  
			
		);
	}
}