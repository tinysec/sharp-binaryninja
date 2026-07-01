using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNType* BNCreateFragmentType(size_t width, const BNTypeWithConfidence* const type, uint64_t offset, BNEndianness endianness)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCreateFragmentType"
        )]
		internal static extern IntPtr BNCreateFragmentType(
			
			// size_t width
		    UIntPtr width   , 
			
			// const BNTypeWithConfidence* const type
		    IntPtr type   , 
			
			// uint64_t offset
		    ulong offset   , 
			
			// BNEndianness endianness
		    Endianness endianness  
		);
	}
}
