using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeBuilderHasTemplateArguments(BNTypeBuilder* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeBuilderHasTemplateArguments"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypeBuilderHasTemplateArguments(
			
			// BNTypeBuilder* type
		    IntPtr type  
			
		);
	}
}