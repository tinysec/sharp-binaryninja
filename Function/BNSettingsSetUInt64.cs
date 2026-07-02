using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSettingsSetUInt64(BNSettings* settings, BNBinaryView* view, BNFunction* func, BNSettingsScope scope, const char* key, uint64_t value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSettingsSetUInt64"
        )]
		internal static extern bool BNSettingsSetUInt64(
			
			// BNSettings* settings
		    IntPtr settings  , 
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNSettingsScope scope
		    SettingsScope scope  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// uint64_t _value
		    ulong _value  
		);
	}
}