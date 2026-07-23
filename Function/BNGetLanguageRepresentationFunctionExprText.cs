using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNDisassemblyTextLine* BNGetLanguageRepresentationFunctionExprText(BNLanguageRepresentationFunction* func, BNHighLevelILFunction* il, uint64_t exprIndex, BNDisassemblySettings* settings, bool asFullAst, BNOperatorPrecedence precedence, bool statement, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetLanguageRepresentationFunctionExprText"
        )]
		internal static extern IntPtr BNGetLanguageRepresentationFunctionExprText(
			
			// BNLanguageRepresentationFunction* func
		    IntPtr func  , 
			
			// BNHighLevelILFunction* il
		    IntPtr il  , 
			
			// uint64_t exprIndex
			HighLevelILExpressionIndex exprIndex  , 
			
			// BNDisassemblySettings* settings
		    IntPtr settings  , 
			
			// bool asFullAst
		    [MarshalAs(UnmanagedType.I1)] bool asFullAst  ,
			
			// BNOperatorPrecedence precedence
		    OperatorPrecedence precedence  , 
			
			// bool statement
		    [MarshalAs(UnmanagedType.I1)] bool statement  ,
			
			// uint64_t* count
		    out ulong count  
		);
	}
}
