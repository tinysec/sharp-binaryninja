using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNDeserializeSettings(BNSettings* settings, const char* contents, BNBinaryView* view, BNFunction* func, BNSettingsScope scope)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNDeserializeSettings"
        )]
		internal static extern bool BNDeserializeSettings(
			
			// BNSettings* settings
		    IntPtr settings  , 
			
			// const char* contents
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string contents  , 
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNSettingsScope scope
		    SettingsScope scope  
		);
	}
}