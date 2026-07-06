using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNShouldSkipTargetAnalysis(BNBinaryView* view, BNArchitectureAndAddress* source, BNFunction* sourceFunc, uint64_t sourceEnd, BNArchitectureAndAddress* target)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNShouldSkipTargetAnalysis"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNShouldSkipTargetAnalysis(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNArchitectureAndAddress* source
		    in BNArchitectureAndAddress source  , 
			
			// BNFunction* sourceFunc
		    IntPtr sourceFunc  , 
			
			// uint64_t sourceEnd
		    ulong sourceEnd  , 
			
			// BNArchitectureAndAddress* target
		    in BNArchitectureAndAddress target  
		);
	}
}