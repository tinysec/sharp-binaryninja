using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeArchiveMergeConflictSuccess(BNTypeArchiveMergeConflict* conflict, const char* value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeArchiveMergeConflictSuccess"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypeArchiveMergeConflictSuccess(
			
			// BNTypeArchiveMergeConflict* conflict
		    IntPtr conflict  , 
			
			// const char* _value
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string _value  
			
		);
	}
}