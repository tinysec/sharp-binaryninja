using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetTypeArchiveCurrentSnapshot(BNTypeArchive* archive, const char* id)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetTypeArchiveCurrentSnapshot"
        )]
		internal static extern void BNSetTypeArchiveCurrentSnapshot(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// const char* id
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string id  
			
		);
	}
}