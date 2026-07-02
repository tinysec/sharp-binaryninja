using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSettingsSetResourceId(BNSettings* settings, const char* resourceId)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSettingsSetResourceId"
        )]
		internal static extern void BNSettingsSetResourceId(
			
			// BNSettings* settings
		    IntPtr settings  , 
			
			// const char* resourceId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string resourceId  
		);
	}
}