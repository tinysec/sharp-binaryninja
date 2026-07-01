using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNFunction* BNGetCalleeForAnalysis(BNFunction* func, BNPlatform* platform, uint64_t addr, bool exact)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetCalleeForAnalysis"
        )]
		internal static extern IntPtr BNGetCalleeForAnalysis(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNPlatform* platform
		    IntPtr platform  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// bool exact
		    bool exact  
			
		);
	}
}