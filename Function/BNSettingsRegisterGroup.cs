using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSettingsRegisterGroup(BNSettings* settings, const char* group, const char* title)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSettingsRegisterGroup"
        )]
		internal static extern bool BNSettingsRegisterGroup(
			
			// BNSettings* settings
		    IntPtr settings  , 
			
			// const char* _group
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string _group  , 
			
			// const char* title
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string title  
		);
	}
}