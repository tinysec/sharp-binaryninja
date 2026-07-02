using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNRemoveTypeBuilderAttribute(BNTypeBuilder* type, const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoveTypeBuilderAttribute"
        )]
		internal static extern void BNRemoveTypeBuilderAttribute(
			
			// BNTypeBuilder* type
		    IntPtr type  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  
			
		);
	}
}