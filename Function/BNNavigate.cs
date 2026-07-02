using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNNavigate(BNFileMetadata* file, const char* view, uint64_t offset)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNNavigate"
        )]
		internal static extern bool BNNavigate(
			
			// BNFileMetadata* file
		    IntPtr file  , 
			
			// const char* view
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string view  , 
			
			// uint64_t offset
		    ulong offset  
		);
	}
}