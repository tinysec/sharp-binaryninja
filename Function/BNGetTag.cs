using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNTag* BNGetTag(BNBinaryView* view, const char* tagId)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTag"
        )]
		internal static extern IntPtr BNGetTag(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* tagId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string tagId  
			
		);
	}
}