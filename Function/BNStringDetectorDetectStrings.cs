using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNStringReference* BNStringDetectorDetectStrings(BNStringDetector* detector, const uint8_t* data, size_t dataLen, size_t blockLen, uint64_t baseAddress, BNStringReference* lastFoundString, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNStringDetectorDetectStrings"
        )]
		internal static extern IntPtr BNStringDetectorDetectStrings(
			
			// BNStringDetector* detector
		    IntPtr detector   , 
			
			// const uint8_t* data
		    IntPtr data   , 
			
			// size_t dataLen
		    UIntPtr dataLen   , 
			
			// size_t blockLen
		    UIntPtr blockLen   , 
			
			// uint64_t baseAddress
		    ulong baseAddress   , 
			
			// BNStringReference* lastFoundString
		    IntPtr lastFoundString   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
