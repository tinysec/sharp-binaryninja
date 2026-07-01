using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// uint64_t BNMediumLevelILGetOperand( BNMediumLevelILFunction* func, size_t offset)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNMediumLevelILGetOperand"
        )]
		internal static extern ulong BNMediumLevelILGetOperand(
			
			// BNMediumLevelILFunction* func
		    IntPtr func   , 
			
			// size_t offset
		    UIntPtr offset  
		);
	}
}
