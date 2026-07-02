using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSettingsUpdateUInt64Property(BNSettings* settings, const char* key, const char* property, uint64_t value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSettingsUpdateUInt64Property"
        )]
		internal static extern bool BNSettingsUpdateUInt64Property(
			
			// BNSettings* settings
		    IntPtr settings  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// const char* property
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string property  , 
			
			// uint64_t _value
		    ulong _value  
			
		);
	}
}