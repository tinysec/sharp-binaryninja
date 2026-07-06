using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetBasicBlockInstructionContainingAddress(BNBasicBlock* block, uint64_t addr, uint64_t* start)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetBasicBlockInstructionContainingAddress"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetBasicBlockInstructionContainingAddress(
			
			// BNBasicBlock* block
		    IntPtr block  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// uint64_t* start
		    out ulong start
		);
	}
}