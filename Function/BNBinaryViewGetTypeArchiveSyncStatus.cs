using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNSyncStatus BNBinaryViewGetTypeArchiveSyncStatus(BNBinaryView* view, const char* typeId)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewGetTypeArchiveSyncStatus"
        )]
		internal static extern SyncStatus BNBinaryViewGetTypeArchiveSyncStatus(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* typeId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string typeId  
		);
	}
}