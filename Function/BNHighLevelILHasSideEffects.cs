using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNHighLevelILHasSideEffects(BNHighLevelILFunction* func, uint64_t exprIndex)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNHighLevelILHasSideEffects"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNHighLevelILHasSideEffects(
			
			// BNHighLevelILFunction* func
		    IntPtr func  , 
			
			// uint64_t exprIndex
			HighLevelILExpressionIndex exprIndex  
		);
	}
}