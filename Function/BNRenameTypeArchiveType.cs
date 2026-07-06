using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRenameTypeArchiveType(BNTypeArchive* archive, const char* id, BNQualifiedName* newName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRenameTypeArchiveType"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRenameTypeArchiveType(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// const char* id
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string id  , 
			
			// BNQualifiedName* newName
		    IntPtr newName  
			
		);
	}
}