using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeContainerGetTypeName(BNTypeContainer* container, const char* typeId, BNQualifiedName* result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeContainerGetTypeName"
        )]
		internal static extern bool BNTypeContainerGetTypeName(
			
			// BNTypeContainer* container
		    IntPtr container  , 
			
			// const char* typeId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string typeId  , 
			
			// BNQualifiedName* result
		    out BNQualifiedName result  
		);
	}
}