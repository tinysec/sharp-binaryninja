using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNUnregisterViewOfType(BNFileMetadata* file, const char* type, BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNUnregisterViewOfType"
        )]
		internal static extern void BNUnregisterViewOfType(
			
			// BNFileMetadata* file
		    IntPtr file  , 
			
			// const char* type
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string type  , 
			
			// BNBinaryView* view
		    IntPtr view  
		);
	}
}