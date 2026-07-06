using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsGNU3MangledString(const char* mangledName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNIsGNU3MangledString"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsGNU3MangledString(
			
			// const char* mangledName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string mangledName  
			
		);
	}
}