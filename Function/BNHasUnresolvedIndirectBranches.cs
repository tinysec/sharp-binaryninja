using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNHasUnresolvedIndirectBranches(BNFunction* func)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNHasUnresolvedIndirectBranches"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNHasUnresolvedIndirectBranches(
			
			// BNFunction* func
		    IntPtr func  
		);
	}
}