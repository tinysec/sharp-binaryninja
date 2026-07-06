using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationIgnoreSnapshot(BNDatabase* database, BNSnapshot* snapshot)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationIgnoreSnapshot"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationIgnoreSnapshot(
			
			// BNDatabase* database
		    IntPtr database  , 
			
			// BNSnapshot* snapshot
		    IntPtr snapshot  
		);
	}
}