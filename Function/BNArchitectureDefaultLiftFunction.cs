using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNArchitectureDefaultLiftFunction(BNLowLevelILFunction* function, BNFunctionLifterContext* context)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNArchitectureDefaultLiftFunction"
        )]
		internal static extern bool BNArchitectureDefaultLiftFunction(
			
			// BNLowLevelILFunction* function
		    IntPtr function   , 
			
			// BNFunctionLifterContext* context
		    IntPtr context  
		);
	}
}
