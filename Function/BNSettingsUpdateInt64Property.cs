using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSettingsUpdateInt64Property(BNSettings* settings, const char* key, const char* property, int64_t value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSettingsUpdateInt64Property"
        )]
		internal static extern bool BNSettingsUpdateInt64Property(
			
			// BNSettings* settings
		    IntPtr settings  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// const char* property
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string property  , 
			
			// int64_t _value
		    long _value  
			
		);
	}
}