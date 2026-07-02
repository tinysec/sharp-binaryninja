using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeContainerRenameType(BNTypeContainer* container, const char* typeId, BNQualifiedName* newName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeContainerRenameType"
        )]
		internal static extern bool BNTypeContainerRenameType(
			
			// BNTypeContainer* container
		    IntPtr container  , 
			
			// const char* typeId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string typeId  , 
			
			// BNQualifiedName* newName
			in BNQualifiedName newName  
		);
	}
}