using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNSymbolDisplayResult BNAddHighLevelILPointerTextToken(BNHighLevelILFunction* func, uint64_t exprIndex, int64_t val, BNHighLevelILTokenEmitter* tokens, BNDisassemblySettings* settings, BNSymbolDisplayType symbolDisplay, BNOperatorPrecedence precedence, bool allowShortString)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAddHighLevelILPointerTextToken"
        )]
		internal static extern SymbolDisplayResult BNAddHighLevelILPointerTextToken(
			
			// BNHighLevelILFunction* func
		    IntPtr func  , 
			
			// uint64_t exprIndex
		    ulong exprIndex  , 
			
			// int64_t val
		    long val  , 
			
			// BNHighLevelILTokenEmitter* tokens
		    IntPtr tokens  , 
			
			// BNDisassemblySettings* settings
		    IntPtr settings  , 
			
			// BNSymbolDisplayType symbolDisplay
		    SymbolDisplayType symbolDisplay  , 
			
			// BNOperatorPrecedence precedence
		    OperatorPrecedence precedence  , 
			
			// bool allowShortString
		    [MarshalAs(UnmanagedType.I1)] bool allowShortString
		);
	}
}
