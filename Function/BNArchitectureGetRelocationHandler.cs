using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNRelocationHandler* BNArchitectureGetRelocationHandler(BNArchitecture* arch, const char* viewName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNArchitectureGetRelocationHandler"
        )]
		internal static extern IntPtr BNArchitectureGetRelocationHandler(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// const char* viewName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string viewName  
		);
	}
}