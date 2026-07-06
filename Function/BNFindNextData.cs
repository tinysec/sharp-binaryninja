using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFindNextData(BNBinaryView* view, uint64_t start, BNDataBuffer* data, uint64_t* result, BNFindFlag flags)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFindNextData"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFindNextData(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// uint64_t start
		    ulong start  , 
			
			// BNDataBuffer* data
		    IntPtr data  , 
			
			// uint64_t* result
		    out ulong result  , 
			
			// BNFindFlag flags
		    FindFlag flags  
		);
	}
}