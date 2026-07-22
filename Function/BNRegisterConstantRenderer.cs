using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNConstantRenderer* BNRegisterConstantRenderer( const char* name, BNCustomConstantRenderer* renderer)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterConstantRenderer"
        )]
		internal static extern IntPtr BNRegisterConstantRenderer(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name   , 
			
			// BNCustomConstantRenderer* renderer
		    in BNCustomConstantRenderer renderer
		);
	}
}
