using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// uint64_t BNGetEntropy(BNBinaryView* view, uint64_t offset, uint64_t len, uint64_t blockSize, float* result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetEntropy"
        )]
		internal static extern ulong BNGetEntropy(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// uint64_t offset
		    ulong offset  , 
			
			// uint64_t len
		    ulong len  , 
			
			// uint64_t blockSize
		    ulong blockSize  ,

			// float* result (caller-allocated buffer the core writes into)
		    [Out] float[] result
		);
	}
}