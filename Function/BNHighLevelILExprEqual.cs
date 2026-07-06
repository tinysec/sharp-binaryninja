using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNHighLevelILExprEqual(BNHighLevelILFunction* leftFunc, uint64_t leftExpr, BNHighLevelILFunction* rightFunc, uint64_t rightExpr)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNHighLevelILExprEqual"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNHighLevelILExprEqual(
			
			// BNHighLevelILFunction* leftFunc
		    IntPtr leftFunc  , 
			
			// uint64_t leftExpr
		    ulong leftExpr  , 
			
			// BNHighLevelILFunction* rightFunc
		    IntPtr rightFunc  , 
			
			// uint64_t rightExpr
		    ulong rightExpr  
			
		);
	}
}