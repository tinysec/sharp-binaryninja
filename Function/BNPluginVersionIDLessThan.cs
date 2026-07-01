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
		internal static extern bool BNPluginVersionIDLessThan(
			
			// BNPlugin* p
		    IntPtr p   , 
			
			// const char* smaller
		    string smaller   , 
			
			// const char* larger
		    string larger  
		);
	}
}
