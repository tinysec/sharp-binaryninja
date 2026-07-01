using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNType* BNCreateFragmentTypeBits(size_t width, const BNTypeWithConfidence* const type, uint64_t originalFragmentOffsetBytes, size_t originalFragmentWidthBytes, BNEndianness endianness, size_t fragmentStartBit, size_t fragmentWidthBits, size_t fragmentTruncatedStartBits, size_t wrapBit)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCreateFragmentTypeBits"
        )]
		internal static extern IntPtr BNCreateFragmentTypeBits(
			
			// size_t width
		    UIntPtr width   , 
			
			// const BNTypeWithConfidence* const type
		    IntPtr type   , 
			
			// uint64_t originalFragmentOffsetBytes
		    ulong originalFragmentOffsetBytes   , 
			
			// size_t originalFragmentWidthBytes
		    UIntPtr originalFragmentWidthBytes   , 
			
			// BNEndianness endianness
		    Endianness endianness   , 
			
			// size_t fragmentStartBit
		    UIntPtr fragmentStartBit   , 
			
			// size_t fragmentWidthBits
		    UIntPtr fragmentWidthBits   , 
			
			// size_t fragmentTruncatedStartBits
		    UIntPtr fragmentTruncatedStartBits   , 
			
			// size_t wrapBit
		    UIntPtr wrapBit  
		);
	}
}
