using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNPossibleValueSet BNPossibleValueSetArithShiftRight(const BNPossibleValueSet* object, const BNPossibleValueSet* other, size_t size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNPossibleValueSetArithShiftRight"
        )]
		internal static extern BNPossibleValueSet BNPossibleValueSetArithShiftRight(
			
			// const BNPossibleValueSet* object
		    IntPtr @object   , 
			
			// const BNPossibleValueSet* other
		    IntPtr other   , 
			
			// size_t size
		    UIntPtr size  
		);
	}
}
