using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNProjectRemoveMetadata(BNProject* project, const char* key)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNProjectRemoveMetadata"
        )]
		internal static extern bool BNProjectRemoveMetadata(
			
			// BNProject* project
		    IntPtr project  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  
			
		);
	}
}