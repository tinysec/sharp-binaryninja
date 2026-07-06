using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSeekLinearViewCursorToCursorPath(BNLinearViewCursor* cursor, BNLinearViewCursor* path)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSeekLinearViewCursorToCursorPath"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSeekLinearViewCursorToCursorPath(
			
			// BNLinearViewCursor* cursor
		    IntPtr cursor  , 
			
			// BNLinearViewCursor* path
		    IntPtr path  
			
		);
	}
}