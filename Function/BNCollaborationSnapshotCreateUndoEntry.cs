using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNCollaborationUndoEntry* BNCollaborationSnapshotCreateUndoEntry(BNCollaborationSnapshot* snapshot, bool hasParent, uint64_t parent, const char* data)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCollaborationSnapshotCreateUndoEntry"
        )]
		internal static extern IntPtr BNCollaborationSnapshotCreateUndoEntry(
			
			// BNCollaborationSnapshot* snapshot
		    IntPtr snapshot  , 
			
			// bool hasParent
		    bool hasParent  , 
			
			// uint64_t parent
		    ulong parent  , 
			
			// const char* data
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string data  
		);
	}
}