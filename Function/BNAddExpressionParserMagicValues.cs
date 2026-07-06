using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNAddExpressionParserMagicValues(BNBinaryView* view, const char** names, uint64_t* values, uint64_t count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddExpressionParserMagicValues"
        )]
		internal static extern void BNAddExpressionParserMagicValues(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char** names: caller-built UTF-8 char** block (string[]
			// elements cannot carry LPUTF8Str, so the wrapper builds the block).
		    IntPtr names  ,
			
			// uint64_t* values
		    ulong[] values  , 
			
			// uint64_t count
		    ulong count  
		);
	}
}