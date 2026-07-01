using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNParseValueLocation( const char* str, BNArchitecture* arch, BNValueLocation* location, char** error)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNParseValueLocation"
        )]
		internal static extern bool BNParseValueLocation(
			
			// const char* str
		    string str   , 
			
			// BNArchitecture* arch
		    IntPtr arch   , 
			
			// BNValueLocation* location
		    IntPtr location   , 
			
			// char** error
		    IntPtr error  
		);
	}
}
