using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNPluginVersionIDLessThan(BNPlugin* p, const char* smaller, const char* larger)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNPluginVersionIDLessThan"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNPluginVersionIDLessThan(
			
			// BNPlugin* p
		    IntPtr p   , 
			
			// const char* smaller
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string smaller   , 
			
			// const char* larger
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string larger  
		);
	}
}
