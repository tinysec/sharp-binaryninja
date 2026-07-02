using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNArchitecture* BNRegisterArchitectureExtension(const char* name, BNArchitecture* @base, BNCustomArchitecture* arch)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterArchitectureExtension"
        )]
		internal static extern IntPtr BNRegisterArchitectureExtension(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNArchitecture* _base
		    IntPtr _base  , 
			
			// BNCustomArchitecture* arch
		    IntPtr arch  
			
		);
	}
}