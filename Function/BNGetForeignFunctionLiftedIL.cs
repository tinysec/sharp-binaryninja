using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNLowLevelILFunction* BNGetForeignFunctionLiftedIL( const BNFunction* func, const BNLogger* logger, const size_t inlinedCallsCount, const uint64_t* inlinedCalls)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetForeignFunctionLiftedIL"
        )]
		internal static extern IntPtr BNGetForeignFunctionLiftedIL(
			
			// const BNFunction* func
		    IntPtr func   , 
			
			// const BNLogger* logger
		    IntPtr logger   , 
			
			// const size_t inlinedCallsCount
		    UIntPtr inlinedCallsCount   , 
			
			// const uint64_t* inlinedCalls
		    IntPtr inlinedCalls  
		);
	}
}
