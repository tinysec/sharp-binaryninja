using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationChangesetSetName(BNCollaborationChangeset* changeset, const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCollaborationChangesetSetName"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationChangesetSetName(
			
			// BNCollaborationChangeset* changeset
		    IntPtr changeset  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  
		);
	}
}