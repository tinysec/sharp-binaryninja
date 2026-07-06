using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationUndoEntryGetParentId(BNCollaborationUndoEntry* undoEntry, uint64_t* parentId)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationUndoEntryGetParentId"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationUndoEntryGetParentId(
			
			// BNCollaborationUndoEntry* undoEntry
		    IntPtr undoEntry  , 
			
			// uint64_t* parentId
		    IntPtr parentId  
		);
	}
}