using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// uint64_t BNLowLevelILGetOperand( BNLowLevelILFunction* func, size_t offset)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNLowLevelILGetOperand"
        )]
		internal static extern ulong BNLowLevelILGetOperand(
			
			// BNLowLevelILFunction* func
		    IntPtr func   , 
			
			// size_t offset
		    UIntPtr offset  
		);
	}
}
