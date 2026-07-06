using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSettingsSetStringList(BNSettings* settings, BNBinaryView* view, BNFunction* func, BNSettingsScope scope, const char* key, const char** value, uint64_t size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSettingsSetStringList"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSettingsSetStringList(
			
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
			
			// const char** _value (UTF-8 block built by the caller; string[] cannot be
			// marshaled as UTF-8 by attribute, so it is passed as a raw pointer)
		    IntPtr _value  ,

			// uint64_t size
		    ulong size
		);
	}
}