using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNNamedTypeReferenceBuilder* BNCreateNamedTypeBuilder(BNNamedTypeReferenceClass cls, const char* id, BNQualifiedName* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateNamedTypeBuilder"
        )]
		internal static extern IntPtr BNCreateNamedTypeBuilder(
			
			// BNNamedTypeReferenceClass cls
		    NamedTypeReferenceClass cls  , 
			
			// const char* id
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string id  , 
			
			// BNQualifiedName* name
		    IntPtr name  
		);
	}
}