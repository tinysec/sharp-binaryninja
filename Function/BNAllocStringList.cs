using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// char** BNAllocStringList(const char** contents, uint64_t size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAllocStringList"
        )]
		internal static extern IntPtr BNAllocStringList(
			
			// const char** contents
		    IntPtr contents  ,
			
			// uint64_t size
		    ulong size  
		);
	}
}
