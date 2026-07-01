using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNVariable* BNGetDefaultParameterOrderingForVariables(BNCallingConvention* cc, const BNVariable* paramVars, const BNType** paramTypes, size_t paramCount, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetDefaultParameterOrderingForVariables"
        )]
		internal static extern IntPtr BNGetDefaultParameterOrderingForVariables(
			
			// BNCallingConvention* cc
		    IntPtr cc   , 
			
			// const BNVariable* paramVars
		    IntPtr paramVars   , 
			
			// const BNType** paramTypes
		    IntPtr paramTypes   , 
			
			// size_t paramCount
		    UIntPtr paramCount   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
