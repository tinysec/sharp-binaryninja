using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNArchitectureRegisterRelocationHandler(BNArchitecture* arch, const char* viewName, BNRelocationHandler* handler)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNArchitectureRegisterRelocationHandler"
        )]
		internal static extern void BNArchitectureRegisterRelocationHandler(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// const char* viewName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string viewName  , 
			
			// BNRelocationHandler* handler
		    IntPtr handler  
		);
	}
}