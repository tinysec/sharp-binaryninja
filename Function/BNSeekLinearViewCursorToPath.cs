using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSeekLinearViewCursorToPath(BNLinearViewCursor* cursor, BNLinearViewObjectIdentifier* ids, uint64_t count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSeekLinearViewCursorToPath"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSeekLinearViewCursorToPath(
			
			// BNLinearViewCursor* cursor
		    IntPtr cursor  , 
			
			// BNLinearViewObjectIdentifier* ids
			BNLinearViewObjectIdentifier[] ids  , 
			
			// uint64_t count
		    ulong count  
		);
	}
}