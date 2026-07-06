using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNGetValueLocationVariableForParameter( const BNValueLocation* location, BNVariable* var, size_t idx)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetValueLocationVariableForParameter"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetValueLocationVariableForParameter(
			
			// const BNValueLocation* location
		    IntPtr location   , 
			
			// BNVariable* var
		    IntPtr var   , 
			
			// size_t idx
		    UIntPtr idx  
		);
	}
}
