using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSaveToFilename(BNBinaryView* view, const char* filename)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSaveToFilename"
        )]
		internal static extern bool BNSaveToFilename(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* filename
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string filename  
		);
	}
}