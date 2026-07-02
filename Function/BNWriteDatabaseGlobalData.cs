using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNWriteDatabaseGlobalData(BNDatabase* database, const char* key, BNDataBuffer* val)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNWriteDatabaseGlobalData"
        )]
		internal static extern bool BNWriteDatabaseGlobalData(
			
			// BNDatabase* database
		    IntPtr database  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// BNDataBuffer* val
		    IntPtr val  
			
		);
	}
}