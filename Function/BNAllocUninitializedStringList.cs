using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// char** BNAllocUninitializedStringList(size_t size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAllocUninitializedStringList"
        )]
		internal static extern IntPtr BNAllocUninitializedStringList(
			
			// size_t size
		    UIntPtr size  
		);
	}
}
