using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNTypeLibrary* BNLookupTypeLibraryByName(BNArchitecture* arch, const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLookupTypeLibraryByName"
        )]
		internal static extern IntPtr BNLookupTypeLibraryByName(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  
		);
	}
}