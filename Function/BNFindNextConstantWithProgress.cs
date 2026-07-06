using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFindNextConstantWithProgress(BNBinaryView* view, uint64_t start, uint64_t end, uint64_t constant, uint64_t* result, BNDisassemblySettings* settings, BNFunctionViewType viewType, void* ctxt, void** progress)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFindNextConstantWithProgress"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFindNextConstantWithProgress(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// uint64_t start
		    ulong start  , 
			
			// uint64_t end
		    ulong end  , 
			
			// uint64_t constant
		    ulong constant  , 
			
			// uint64_t* result
		    out ulong result  , 
			
			// BNDisassemblySettings* settings
		    IntPtr settings  , 
			
			// BNFunctionViewType viewType
		    in BNFunctionViewType viewType  , 
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void* progress
		    IntPtr progress  
		);
	}
}