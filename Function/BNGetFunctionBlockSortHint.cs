using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNGetFunctionBlockSortHint( BNFunction* func, BNArchitecture* arch, uint64_t addr, int64_t* result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetFunctionBlockSortHint"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetFunctionBlockSortHint(
			
			// BNFunction* func
		    IntPtr func   , 
			
			// BNArchitecture* arch
		    IntPtr arch   , 
			
			// uint64_t addr
		    ulong addr   , 
			
			// int64_t* result
		    out long result
		);
	}
}
