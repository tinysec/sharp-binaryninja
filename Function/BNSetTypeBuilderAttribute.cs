using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetTypeBuilderAttribute(BNTypeBuilder* type, const char* name, const char* value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetTypeBuilderAttribute"
        )]
		internal static extern void BNSetTypeBuilderAttribute(
			
			// BNTypeBuilder* type
		    IntPtr type  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// const char* _value
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string _value  
			
		);
	}
}