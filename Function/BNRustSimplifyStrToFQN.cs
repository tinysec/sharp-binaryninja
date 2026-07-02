using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNQualifiedName BNRustSimplifyStrToFQN(const char* const, bool)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRustSimplifyStrToFQN"
        )]
		internal static extern BNQualifiedName BNRustSimplifyStrToFQN(
			
			// const char* const
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string @const   , 
			
			// bool
		    bool arg1  
		);
	}
}
