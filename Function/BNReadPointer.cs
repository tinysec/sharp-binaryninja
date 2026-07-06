using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNReadPointer(BNBinaryView* view, BNBinaryReader* stream, uint64_t* result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNReadPointer"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNReadPointer(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNBinaryReader* stream
		    IntPtr stream  , 
			
			// uint64_t* result
		    out ulong result  
		);
	}
}