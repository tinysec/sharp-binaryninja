using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSettingsSetBool(BNSettings* settings, BNBinaryView* view, BNFunction* func, BNSettingsScope scope, const char* key, bool value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSettingsSetBool"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSettingsSetBool(
			
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
			
			// bool _value
		    bool _value  
		);
	}
}