using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNArchitectureLiftFunction(BNArchitecture* arch, BNLowLevelILFunction* function, BNFunctionLifterContext* context)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNArchitectureLiftFunction"
        )]
		internal static extern bool BNArchitectureLiftFunction(
			
			// BNArchitecture* arch
		    IntPtr arch   , 
			
			// BNLowLevelILFunction* function
		    IntPtr function   , 
			
			// BNFunctionLifterContext* context
		    IntPtr context  
		);
	}
}
