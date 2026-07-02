using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNTag* BNCreateTag(BNTagType* type, const char* data)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateTag"
        )]
		internal static extern IntPtr BNCreateTag(
			
			// BNTagType* type
		    IntPtr type  , 
			
			// const char* data
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string data  
		);
	}
}