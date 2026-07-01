using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetHighLevelILDerivedStringReferenceForExpr( BNHighLevelILFunction* func, size_t expr, BNDerivedString* str)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetHighLevelILDerivedStringReferenceForExpr"
        )]
		internal static extern void BNSetHighLevelILDerivedStringReferenceForExpr(
			
			// BNHighLevelILFunction* func
		    IntPtr func   , 
			
			// size_t expr
		    UIntPtr expr   , 
			
			// BNDerivedString* str
		    IntPtr str  
		);
	}
}
