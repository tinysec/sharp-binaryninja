using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSettingsUpdateStringListProperty(BNSettings* settings, const char* key, const char* property, const char** value, uint64_t size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSettingsUpdateStringListProperty"
        )]
		internal static extern bool BNSettingsUpdateStringListProperty(
			
			// BNSettings* settings
		    IntPtr settings  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// const char* property
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string property  , 
			
			// const char** _value (UTF-8 block built by the caller; string[] cannot be
			// marshaled as UTF-8 by attribute, so it is passed as a raw pointer)
		    IntPtr _value  ,

			// uint64_t size
		    ulong size

		);
	}
}