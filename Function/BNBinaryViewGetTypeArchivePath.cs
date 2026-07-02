using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// char* BNBinaryViewGetTypeArchivePath(BNBinaryView* view, const char* id)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewGetTypeArchivePath"
        )]
		internal static extern IntPtr BNBinaryViewGetTypeArchivePath(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* id
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string id  
		);
	}
}