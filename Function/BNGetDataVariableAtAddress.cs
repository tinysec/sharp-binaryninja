using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetDataVariableAtAddress(BNBinaryView* view, uint64_t addr, BNDataVariable* var)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetDataVariableAtAddress"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetDataVariableAtAddress(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// BNDataVariable* _var
		    out BNDataVariable var  
		);
	}
}