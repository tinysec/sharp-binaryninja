using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNUnregisterObjectRefDebugTrace(const char* typeName, void* trace)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNUnregisterObjectRefDebugTrace"
        )]
		internal static extern void BNUnregisterObjectRefDebugTrace(
			
			// const char* typeName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string typeName  , 
			
			// void* trace
		    IntPtr trace  
			
		);
	}
}