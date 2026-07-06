using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNComponentRemoveDataVariable(BNComponent* component, uint64_t address)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNComponentRemoveDataVariable"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNComponentRemoveDataVariable(
			
			// BNComponent* component
		    IntPtr component  , 
			
			// uint64_t address
		    ulong address  
		);
	}
}