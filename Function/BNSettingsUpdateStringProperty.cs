using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSettingsUpdateStringProperty(BNSettings* settings, const char* key, const char* property, const char* value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSettingsUpdateStringProperty"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSettingsUpdateStringProperty(
			
			// BNSettings* settings
		    IntPtr settings  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// const char* property
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string property  , 
			
			// const char* _value
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string _value  
			
		);
	}
}