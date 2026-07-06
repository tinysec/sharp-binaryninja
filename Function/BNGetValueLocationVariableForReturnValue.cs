using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNGetValueLocationVariableForReturnValue(const BNValueLocation* location, BNVariable* var)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetValueLocationVariableForReturnValue"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetValueLocationVariableForReturnValue(
			
			// const BNValueLocation* location
		    IntPtr location   , 
			
			// BNVariable* var
		    IntPtr var  
		);
	}
}
