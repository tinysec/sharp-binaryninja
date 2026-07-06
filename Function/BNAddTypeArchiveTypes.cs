using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAddTypeArchiveTypes(BNTypeArchive* archive, BNQualifiedNameAndType* types, uint64_t count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAddTypeArchiveTypes"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNAddTypeArchiveTypes(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// BNQualifiedNameAndType* types
		    IntPtr types  , 
			
			// uint64_t count
		    ulong count  
		);
	}
}