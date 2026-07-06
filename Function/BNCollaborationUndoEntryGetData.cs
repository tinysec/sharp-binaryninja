using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationUndoEntryGetData(BNCollaborationUndoEntry* undoEntry, const char** data)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCollaborationUndoEntryGetData"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationUndoEntryGetData(
			
			// BNCollaborationUndoEntry* undoEntry
		    IntPtr undoEntry  , 
			
			// const char** data
		    string[] data  
		);
	}
}