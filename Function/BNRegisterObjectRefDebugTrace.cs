using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void* BNRegisterObjectRefDebugTrace(const char* typeName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterObjectRefDebugTrace"
        )]
		internal static extern IntPtr BNRegisterObjectRefDebugTrace(
			
			// const char* typeName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string typeName  
			
		);
	}
}