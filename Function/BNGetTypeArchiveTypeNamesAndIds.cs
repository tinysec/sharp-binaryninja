using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetTypeArchiveTypeNamesAndIds(BNTypeArchive* archive, const char* snapshot, BNQualifiedName** names, const char*** ids, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTypeArchiveTypeNamesAndIds"
        )]
		internal static extern bool BNGetTypeArchiveTypeNamesAndIds(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// const char* snapshot
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string snapshot  , 
			
			// BNQualifiedName** names
		    IntPtr names  , 
			
			// const char*** ids
		    IntPtr ids  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}