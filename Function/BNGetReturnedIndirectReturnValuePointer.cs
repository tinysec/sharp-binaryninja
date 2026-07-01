using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNGetReturnedIndirectReturnValuePointer(BNCallingConvention* cc, BNVariable* outVar)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetReturnedIndirectReturnValuePointer"
        )]
		internal static extern bool BNGetReturnedIndirectReturnValuePointer(
			
			// BNCallingConvention* cc
		    IntPtr cc   , 
			
			// BNVariable* outVar
		    IntPtr outVar  
		);
	}
}
