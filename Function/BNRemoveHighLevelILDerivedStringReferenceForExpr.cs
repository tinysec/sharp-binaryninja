using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNRemoveHighLevelILDerivedStringReferenceForExpr(BNHighLevelILFunction* func, size_t expr)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNRemoveHighLevelILDerivedStringReferenceForExpr"
        )]
		internal static extern void BNRemoveHighLevelILDerivedStringReferenceForExpr(
			
			// BNHighLevelILFunction* func
		    IntPtr func   , 
			
			// size_t expr
		    UIntPtr expr  
		);
	}
}
