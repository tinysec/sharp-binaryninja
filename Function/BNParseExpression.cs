using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNParseExpression(BNBinaryView* view, const char* expression, uint64_t* offset, uint64_t here, const char** errorString)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNParseExpression"
        )]
		internal static extern bool BNParseExpression(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* expression
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string expression  , 
			
			// uint64_t* offset
		    out ulong offset  , 
			
			// uint64_t here
		    ulong here  , 
			
			// const char** errorString
		    out IntPtr errorString  
		);
	}
}