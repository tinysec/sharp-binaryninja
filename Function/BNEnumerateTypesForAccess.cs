using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNEnumerateTypesForAccess(BNType* type, BNBinaryView* data, uint64_t offset, uint64_t size, uint8_t baseConfidence, void** terminal, void* ctxt)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNEnumerateTypesForAccess"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNEnumerateTypesForAccess(
			
			// BNType* type
		    IntPtr type  , 
			
			// BNBinaryView* data
		    IntPtr data  , 
			
			// uint64_t offset
		    ulong offset  , 
			
			// uint64_t size
		    ulong size  , 
			
			// uint8_t baseConfidence
		    byte baseConfidence  , 
			
			// void** terminal
		    IntPtr terminal  , 
			
			// void* ctxt
		    IntPtr ctxt  
			
		);
	}
}