using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNPossibleValueSet BNPossibleValueSetShiftLeft(const BNPossibleValueSet* object, const BNPossibleValueSet* other, size_t size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNPossibleValueSetShiftLeft"
        )]
		internal static extern BNPossibleValueSet BNPossibleValueSetShiftLeft(
			
			// const BNPossibleValueSet* object
		    IntPtr @object   , 
			
			// const BNPossibleValueSet* other
		    IntPtr other   , 
			
			// size_t size
		    UIntPtr size  
		);
	}
}
