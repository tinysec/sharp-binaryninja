using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAddTypeMemberTokens(BNType* type, BNBinaryView* data, BNInstructionTextToken** tokens, uint64_t* tokenCount, int64_t offset, const char*** nameList, uint64_t* nameCount, uint64_t size, bool indirect, BNFieldResolutionInfo* info)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAddTypeMemberTokens"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNAddTypeMemberTokens(
			
			// BNType* type
		    IntPtr type  , 
			
			// BNBinaryView* data
		    IntPtr data  , 
			
			// BNInstructionTextToken** tokens
		    IntPtr tokens  , 
			
			// uint64_t* tokenCount
		    IntPtr tokenCount  , 
			
			// int64_t offset
		    long offset  , 
			
			// const char*** nameList
		    IntPtr nameList  , 
			
			// uint64_t* nameCount
		    IntPtr nameCount  , 
			
			// uint64_t size
		    ulong size  , 
			
			// bool indirect
		    bool indirect  , 
			
			// BNFieldResolutionInfo* info
		    IntPtr info  
		);
	}
}