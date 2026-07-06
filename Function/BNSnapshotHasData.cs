using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSnapshotHasData(BNDatabase* db, int64_t id)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSnapshotHasData"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSnapshotHasData(
			
			// BNDatabase* db
		    IntPtr db  , 
			
			// int64_t id
		    long id  
			
		);
	}
}