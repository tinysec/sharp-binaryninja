using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsAlwaysBranchPatchAvailable(BNBinaryView* view, BNArchitecture* arch, uint64_t addr)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsAlwaysBranchPatchAvailable"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsAlwaysBranchPatchAvailable(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// uint64_t addr
		    ulong addr  
		);
	}
}