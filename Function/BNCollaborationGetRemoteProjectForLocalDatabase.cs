using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationGetRemoteProjectForLocalDatabase(BNDatabase* database, BNRemoteProject** result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationGetRemoteProjectForLocalDatabase"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationGetRemoteProjectForLocalDatabase(
			
			// BNDatabase* database
		    IntPtr database  , 
			
			// BNRemoteProject** result
		    IntPtr result  
		);
	}
}