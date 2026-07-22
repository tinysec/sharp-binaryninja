using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// const char* BNTypeArchiveNewSnapshotTransaction(BNTypeArchive* archive, void** func, void* context, const char** parents, uint64_t parentCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeArchiveNewSnapshotTransaction"
        )]
		internal static extern IntPtr BNTypeArchiveNewSnapshotTransaction(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// void** func
		    IntPtr func  , 
			
			// void* context
		    IntPtr context  , 
			
			// const char** parents
		    IntPtr parents,
			
			// uint64_t parentCount
		    ulong parentCount  
			
		);
	}
}
