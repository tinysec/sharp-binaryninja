using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNBinaryViewGetAssociatedTypeArchiveTypeTarget(BNBinaryView* view, const char* typeId, const char** archiveId, const char** archiveTypeId)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewGetAssociatedTypeArchiveTypeTarget"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNBinaryViewGetAssociatedTypeArchiveTypeTarget(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* typeId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string typeId  , 
			
			// const char** archiveId
		    out IntPtr archiveId  , 
			
			// const char** archiveTypeId
		    out IntPtr archiveTypeId  
		);
	}
}