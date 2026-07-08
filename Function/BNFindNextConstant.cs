using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFindNextConstant(BNBinaryView* view, uint64_t start, uint64_t constant, uint64_t* result, BNDisassemblySettings* settings, BNFunctionViewType viewType)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFindNextConstant"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFindNextConstant(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// uint64_t start
		    ulong start  , 
			
			// uint64_t constant
		    ulong constant  , 
			
			// uint64_t* result
		    out ulong result  , 
			
			// BNDisassemblySettings* settings
		    IntPtr settings  , 
			
			// BNFunctionViewType viewType
		    BNFunctionViewType viewType  
		);
	}
}