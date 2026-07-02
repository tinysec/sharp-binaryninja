using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNBinaryViewRemoveMetadata(BNBinaryView* view, const char* key)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewRemoveMetadata"
        )]
		internal static extern void BNBinaryViewRemoveMetadata(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  
		);
	}
}