using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNProjectStoreMetadata(BNProject* project, const char* key, BNMetadata* value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNProjectStoreMetadata"
        )]
		internal static extern bool BNProjectStoreMetadata(
			
			// BNProject* project
		    IntPtr project  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// BNMetadata* _value
		    IntPtr _value  
			
		);
	}
}