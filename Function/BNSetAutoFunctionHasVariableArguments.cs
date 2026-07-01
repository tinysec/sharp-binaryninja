using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetAutoFunctionHasVariableArguments(BNFunction* func, BNBoolWithConfidence* varArgs)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetAutoFunctionHasVariableArguments"
        )]
		internal static extern void BNSetAutoFunctionHasVariableArguments(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNBoolWithConfidence* varArgs
		    IntPtr varArgs  
			
		);
	}
}