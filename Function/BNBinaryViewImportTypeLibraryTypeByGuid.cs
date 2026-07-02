using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNType* BNBinaryViewImportTypeLibraryTypeByGuid(BNBinaryView* view, const char* guid)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewImportTypeLibraryTypeByGuid"
        )]
		internal static extern IntPtr BNBinaryViewImportTypeLibraryTypeByGuid(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* guid
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string guid  
		);
	}
}