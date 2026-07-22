using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNResolveStructureMemberOrBaseMember(BNStructure* s, BNBinaryView* data,
		/// uint64_t offset, size_t size, void* callbackContext, resolveFunc,
		/// bool memberIndexHintValid, size_t memberIndexHint)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNResolveStructureMemberOrBaseMember"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNResolveStructureMemberOrBaseMember(
			
			// BNStructure* s
		    IntPtr s  , 
			
			// BNBinaryView* data
		    IntPtr data  , 
			
			// uint64_t offset
		    ulong offset  , 
			
			// uint64_t size
		    ulong size  , 
			
			// void* callbackContext
		    IntPtr callbackContext  , 
			
			// Native resolution callback function pointer
		    IntPtr resolveFunc  , 
			
			// bool memberIndexHintValid
		    [MarshalAs(UnmanagedType.I1)] bool memberIndexHintValid  ,
			
			// uint64_t memberIndexHint
		    ulong memberIndexHint  
			
		);
	}
}
