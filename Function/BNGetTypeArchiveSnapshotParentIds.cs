using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// const char** BNGetTypeArchiveSnapshotParentIds(BNTypeArchive* archive, const char* id, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTypeArchiveSnapshotParentIds"
        )]
		internal static extern IntPtr BNGetTypeArchiveSnapshotParentIds(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// const char* id
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string id  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}