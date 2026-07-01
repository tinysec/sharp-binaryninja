using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// uint64_t BNHighLevelILGetOperand( BNHighLevelILFunction* func, size_t offset)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNHighLevelILGetOperand"
        )]
		internal static extern ulong BNHighLevelILGetOperand(
			
			// BNHighLevelILFunction* func
		    IntPtr func   , 
			
			// size_t offset
		    UIntPtr offset  
		);
	}
}
