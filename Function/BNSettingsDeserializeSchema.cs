using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSettingsDeserializeSchema(BNSettings* settings, const char* schema, BNSettingsScope scope, bool merge)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSettingsDeserializeSchema"
        )]
		internal static extern bool BNSettingsDeserializeSchema(
			
			// BNSettings* settings
		    IntPtr settings  , 
			
			// const char* schema
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string schema  , 
			
			// BNSettingsScope scope
		    SettingsScope scope  , 
			
			// bool merge
		    bool merge  
		);
	}
}