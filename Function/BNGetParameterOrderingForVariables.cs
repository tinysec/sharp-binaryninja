using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNVariable* BNGetParameterOrderingForVariables(BNCallingConvention* cc, BNBinaryView* view, const BNVariable* paramVars, const BNType** paramTypes, size_t paramCount, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetParameterOrderingForVariables"
        )]
		internal static extern IntPtr BNGetParameterOrderingForVariables(
			
			// BNCallingConvention* cc
		    IntPtr cc   , 
			
			// BNBinaryView* view
		    IntPtr view   , 
			
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
