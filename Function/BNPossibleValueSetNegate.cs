using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNPossibleValueSet BNPossibleValueSetNegate(const BNPossibleValueSet* object, size_t size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNPossibleValueSetNegate"
        )]
		internal static extern BNPossibleValueSet BNPossibleValueSetNegate(
			
			// const BNPossibleValueSet* object
		    IntPtr @object   , 
			
			// size_t size
		    UIntPtr size  
		);
	}
}
