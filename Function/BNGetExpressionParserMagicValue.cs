using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetExpressionParserMagicValue(BNBinaryView* view, const char* name, uint64_t* value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetExpressionParserMagicValue"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetExpressionParserMagicValue(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// uint64_t* value
		    out ulong value  
		);
	}
}