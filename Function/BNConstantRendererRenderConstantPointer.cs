using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNConstantRendererRenderConstantPointer(BNConstantRenderer* renderer, BNHighLevelILFunction* il, size_t exprIndex, BNType* type, int64_t val, BNHighLevelILTokenEmitter* tokens, BNDisassemblySettings* settings, BNSymbolDisplayType symbolDisplay, BNOperatorPrecedence precedence)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNConstantRendererRenderConstantPointer"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNConstantRendererRenderConstantPointer(
			
			// BNConstantRenderer* renderer
		    IntPtr renderer   , 
			
			// BNHighLevelILFunction* il
		    IntPtr il   , 
			
			// size_t exprIndex
		    UIntPtr exprIndex   , 
			
			// BNType* type
		    IntPtr type   , 
			
			// int64_t val
		    long val   , 
			
			// BNHighLevelILTokenEmitter* tokens
		    IntPtr tokens   , 
			
			// BNDisassemblySettings* settings
		    IntPtr settings   , 
			
			// BNSymbolDisplayType symbolDisplay
		    SymbolDisplayType symbolDisplay   , 
			
			// BNOperatorPrecedence precedence
		    OperatorPrecedence precedence  
		);
	}
}
