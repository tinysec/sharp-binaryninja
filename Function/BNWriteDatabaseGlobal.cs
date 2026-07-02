using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNWriteDatabaseGlobal(BNDatabase* database, const char* key, const char* val)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNWriteDatabaseGlobal"
        )]
		internal static extern bool BNWriteDatabaseGlobal(
			
			// BNDatabase* database
		    IntPtr database  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// const char* val
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string val  
			
		);
	}
}