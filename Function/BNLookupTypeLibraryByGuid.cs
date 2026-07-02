using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNTypeLibrary* BNLookupTypeLibraryByGuid(BNArchitecture* arch, const char* guid)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLookupTypeLibraryByGuid"
        )]
		internal static extern IntPtr BNLookupTypeLibraryByGuid(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// const char* guid
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string guid  
		);
	}
}