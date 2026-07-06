using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNNotifyProgressForDownloadInstance(BNDownloadInstance* instance, uint64_t progress, uint64_t total)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNNotifyProgressForDownloadInstance"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNNotifyProgressForDownloadInstance(
			
			// BNDownloadInstance* instance
		    IntPtr instance  , 
			
			// uint64_t progress
		    ulong progress  , 
			
			// uint64_t total
		    ulong total  
			
		);
	}
}