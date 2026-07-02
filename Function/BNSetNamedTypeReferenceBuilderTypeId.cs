using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetNamedTypeReferenceBuilderTypeId(BNNamedTypeReferenceBuilder* s, const char* id)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetNamedTypeReferenceBuilderTypeId"
        )]
		internal static extern void BNSetNamedTypeReferenceBuilderTypeId(
			
			// BNNamedTypeReferenceBuilder* s
		    IntPtr s  , 
			
			// const char* id
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string id  
			
		);
	}
}