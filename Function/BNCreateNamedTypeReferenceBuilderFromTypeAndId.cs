using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNTypeBuilder* BNCreateNamedTypeReferenceBuilderFromTypeAndId(const char* id, BNQualifiedName* name, BNType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateNamedTypeReferenceBuilderFromTypeAndId"
        )]
		internal static extern IntPtr BNCreateNamedTypeReferenceBuilderFromTypeAndId(
			
			// const char* id
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string id  , 
			
			// BNQualifiedName* name
		    IntPtr name  , 
			
			// BNType* type
		    IntPtr type  
		);
	}
}