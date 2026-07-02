using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNBinaryViewGetAssociatedTypeArchiveTypeSource(BNBinaryView* view, const char* archiveId, const char* archiveTypeId, const char** typeId)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewGetAssociatedTypeArchiveTypeSource"
        )]
		internal static extern bool BNBinaryViewGetAssociatedTypeArchiveTypeSource(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* archiveId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string archiveId  , 
			
			// const char* archiveTypeId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string archiveTypeId  , 
			
			// const char** typeId
		    string[] typeId  
		);
	}
}