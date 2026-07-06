using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationGetRemoteFileForLocalDatabase(BNDatabase* database, BNRemoteFile** result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationGetRemoteFileForLocalDatabase"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationGetRemoteFileForLocalDatabase(
			
			// BNDatabase* database
		    IntPtr database  , 
			
			// BNRemoteFile** result
		    IntPtr result  
		);
	}
}