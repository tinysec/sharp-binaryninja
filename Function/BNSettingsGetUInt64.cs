using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// uint64_t BNSettingsGetUInt64(BNSettings* settings, const char* key, BNBinaryView* view, BNFunction* func, BNSettingsScope* scope)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSettingsGetUInt64"
        )]
		internal static extern ulong BNSettingsGetUInt64(
			
			// BNSettings* settings
		    IntPtr settings  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNSettingsScope* scope
			out SettingsScope scope  
		);
	}
}