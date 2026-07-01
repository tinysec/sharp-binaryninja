using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNGetHighLevelILDerivedStringReferenceForExpr( BNHighLevelILFunction* func, size_t expr, BNDerivedString* out)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetHighLevelILDerivedStringReferenceForExpr"
        )]
		internal static extern bool BNGetHighLevelILDerivedStringReferenceForExpr(
			
			// BNHighLevelILFunction* func
		    IntPtr func   , 
			
			// size_t expr
		    UIntPtr expr   , 
			
			// BNDerivedString* out
		    IntPtr @out  
		);
	}
}
