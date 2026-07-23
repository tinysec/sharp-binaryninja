using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNHighLevelILTokenEmitterRestoreCurrentExpr(BNHighLevelILTokenEmitter* emitter, BNTokenEmitterExpr expr)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNHighLevelILTokenEmitterRestoreCurrentExpr"
        )]
		internal static extern void BNHighLevelILTokenEmitterRestoreCurrentExpr(
			
			// BNHighLevelILTokenEmitter* emitter
		    IntPtr emitter  , 
			
			// BNTokenEmitterExpr expr
		    BNTokenEmitterExpr expr
		);
	}
}
