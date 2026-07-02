using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// const char** BNSettingsGetStringList(BNSettings* settings, const char* key, BNBinaryView* view, BNFunction* func, BNSettingsScope* scope, uint64_t* inoutSize)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSettingsGetStringList"
        )]
		internal static extern IntPtr BNSettingsGetStringList(
			
			// BNSettings* settings
		    IntPtr settings  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNSettingsScope* scope
			out SettingsScope scope  , 
			
			// uint64_t* inoutSize
		    out ulong inoutSize  
		);
	}
}