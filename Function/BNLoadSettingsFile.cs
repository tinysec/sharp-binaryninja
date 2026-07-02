using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNLoadSettingsFile(BNSettings* settings, const char* fileName, BNSettingsScope scope, BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLoadSettingsFile"
        )]
		internal static extern bool BNLoadSettingsFile(
			
			// BNSettings* settings
		    IntPtr settings  , 
			
			// const char* fileName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string fileName  , 
			
			// BNSettingsScope scope
		    SettingsScope scope  , 
			
			// BNBinaryView* view
		    IntPtr view  
		);
	}
}